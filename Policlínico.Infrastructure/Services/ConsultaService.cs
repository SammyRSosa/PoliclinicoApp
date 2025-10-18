using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.Infrastructure.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly PoliclínicoDbContext _context;
        private readonly IMapper _mapper;

        public ConsultaService(PoliclínicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ConsultaReadDto> CreateAsync(ConsultaCreateDto dto)
        {
            // Validaciones básicas
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var paciente = await _context.Pacientes.FindAsync(dto.PacienteId);
            if (paciente == null) throw new InvalidOperationException("Paciente no existe.");

            var medicoPrincipal = await _context.Trabajadores.FindAsync(dto.DoctorPrincipalId);
            if (medicoPrincipal == null) throw new InvalidOperationException("Médico principal no existe.");
            if (!string.Equals(medicoPrincipal.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("El médico principal debe tener cargo 'Doctor'.");

            // Si es Programada requiere departamento y puesto
            if (string.Equals(dto.TipoConsulta, "Programada Externa", StringComparison.OrdinalIgnoreCase))
            {
                if (!dto.PuestoMedicoId.HasValue)
                    throw new InvalidOperationException("Para consultas programadas debe indicar DepartamentoId y PuestoMedicoId.");

                var depExists = await _context.Departamentos.AnyAsync(d => d.IdDepartamento == dto.DepartamentoAtiendeId);
                if (!depExists) throw new InvalidOperationException("Departamento no existe.");

                var puestoExists = await _context.PuestosMedicos.AnyAsync(p => p.IdPuesto == dto.PuestoMedicoId.Value);
                if (!puestoExists) throw new InvalidOperationException("Puesto médico no existe.");
            }

            if (string.Equals(dto.TipoConsulta, "Programada Departamento", StringComparison.OrdinalIgnoreCase))
            {
                if (!dto.PuestoMedicoId.HasValue)
                    throw new InvalidOperationException("Para consultas programadas debe indicar DepartamentoId y PuestoMedicoId.");

                var depExists = await _context.Departamentos.AnyAsync(d => d.IdDepartamento == dto.DepartamentoAtiendeId);
                if (!depExists) throw new InvalidOperationException("Departamento no existe.");

                var dep2Exists = await _context.Departamentos.AnyAsync(d => d.IdDepartamento == dto.PuestoMedicoId);
                if (!dep2Exists) throw new InvalidOperationException("Puesto médico no existe.");
            }


            // Si el usuario quiso añadir diagnóstico en creación se valida la consistencia:
            // - No se puede añadir diagnóstico si la consulta queda en Pendiente o EnCurso
            // - Si contiene diagnóstico la consulta debe quedar Finalizada
            // Decidimos el estado ahora basado en fecha y diagnostico después se valida.

            var nowDate = DateTime.UtcNow.Date;
            var fecha = dto.FechaConsulta.Date;

            string estado;
            if (!string.IsNullOrWhiteSpace(dto.Diagnostico))
            {
                // si tiene diagnóstico, la fecha no puede ser futura.
                if (dto.FechaConsulta.Date > nowDate)
                    throw new InvalidOperationException("No puede añadir diagnóstico a una consulta con fecha futura (Pendiente).");

                estado = "Finalizada";
            }
            else
            {
                if (dto.FechaConsulta.Date > nowDate) estado = "Pendiente";
                else if (dto.FechaConsulta.Date < nowDate) estado = "EnCurso";
                else estado = "EnCurso"; // misma fecha -> en curso
            }

            // Validar doctores participantes (si hay) — si consulta tiene departamento, los doctores deben pertenecer a ese departamento (asignación activa)
            var doctores = new List<Trabajador>();
            if (dto.DoctoresParticipantesIds != null && dto.DoctoresParticipantesIds.Count > 0)
            {
                foreach (var docId in dto.DoctoresParticipantesIds.Distinct())
                {
                    var doc = await _context.Trabajadores.FindAsync(docId);
                    if (doc == null) throw new InvalidOperationException($"Doctor participante (id={docId}) no existe.");
                    if (!string.Equals(doc.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException($"Trabajador id={docId} no tiene cargo 'Doctor'.");

                    // si es programada y hay departamento, validate that doctor is assigned currently to that department
                    if (dto.DepartamentoAtiendeId > 0)
                    {
                        var assigned = await _context.Asignaciones
                            .AnyAsync(a => a.TrabajadorId == docId && a.DepartamentoId == dto.DepartamentoAtiendeId && a.FechaFin == null);
                        if (!assigned)
                            throw new InvalidOperationException($"Doctor id={docId} no está asignado actualmente al departamento {dto.DepartamentoAtiendeId}.");
                    }

                    doctores.Add(doc);
                }
            }

            // Crear entidad
            var consulta = new Consulta
            {
                Tipo = dto.TipoConsulta,
                FechaConsulta = dto.FechaConsulta,
                Diagnostico = dto.Diagnostico,
                PacienteId = dto.PacienteId,
                MedicoPrincipalId = dto.DoctorPrincipalId,
                DepartamentoId = dto.DepartamentoAtiendeId,
                PuestoMedicoId = dto.PuestoMedicoId,
                Estado = estado,
                FechaCreacion = DateTime.UtcNow
            };

            // Attach doctores (many-to-many)
            foreach (var d in doctores)
                consulta.Doctores.Add(d);

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            // Mapear y devolver
            // incluir nombres de paciente y medico principal
            await _context.Entry(consulta).Reference(c => c.Paciente).LoadAsync();
            await _context.Entry(consulta).Reference(c => c.MedicoPrincipal).LoadAsync();
            if (consulta.DepartamentoId > 0) await _context.Entry(consulta).Reference(c => c.Departamento).LoadAsync();
            await _context.Entry(consulta).Collection(c => c.Doctores).LoadAsync();

            var dtoRead = _mapper.Map<ConsultaReadDto>(consulta);
            return dtoRead;
        }

        public async Task<IEnumerable<ConsultaReadDto>> GetAllAsync()
        {
            var list = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.Departamento)
                .Include(c => c.Doctores)
                .ToListAsync();

            return _mapper.Map<List<ConsultaReadDto>>(list);
        }

        public async Task<ConsultaReadDto?> GetByIdAsync(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.Departamento)
                .Include(c => c.Doctores)
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null) return null;
            return _mapper.Map<ConsultaReadDto>(consulta);
        }

        public async Task<ConsultaReadDto> UpdateAsync(int id, ConsultaUpdateDto dto)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Doctores)
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null) throw new InvalidOperationException("Consulta no encontrada.");

            // Cambiar fecha (afecta estado)
            consulta.FechaConsulta = (DateTime)dto.FechaConsulta;

            var nowDate = DateTime.UtcNow.Date;
            var fecha = dto.FechaConsulta;

            // Si el DTO trae diagnóstico intenta aplicarlo -> usar AddDiagnosticoAsync para reglas específicas
            if (!string.IsNullOrWhiteSpace(dto.Diagnostico))
            {
                // adding diagnostico implies finalizada (regla); date must not be in future.
                if (dto.FechaConsulta > nowDate)
                    throw new InvalidOperationException("No puede añadir diagnóstico a una consulta con fecha futura (Pendiente).");

                consulta.Diagnostico = dto.Diagnostico;
                consulta.Estado = "Finalizada";
            }
            else
            {
                // recalcular estado por fecha y diagnostico actual
                if (!string.IsNullOrWhiteSpace(consulta.Diagnostico))
                {
                    consulta.Estado = "Finalizada";
                }
                else
                {
                    if (fecha > nowDate) consulta.Estado = "Pendiente";
                    else consulta.Estado = "EnCurso";
                }
            }

            // actualizar doctores participantes si se proporcionan
            if (dto.DoctoresParticipantesIds != null)
            {
                // limpiar y volver a agregar
                consulta.Doctores.Clear();
                foreach (var docId in dto.DoctoresParticipantesIds.Distinct())
                {
                    var doc = await _context.Trabajadores.FindAsync(docId);
                    if (doc == null) throw new InvalidOperationException($"Doctor id={docId} no existe.");
                    if (!string.Equals(doc.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException($"Trabajador id={docId} no tiene cargo 'Doctor'.");
                    // si consulta tiene departamento validar asignacion
                    if (consulta.DepartamentoId > 0)
                    {
                        var assigned = await _context.Asignaciones
                            .AnyAsync(a => a.TrabajadorId == docId && a.DepartamentoId == consulta.DepartamentoId && a.FechaFin == null);
                        if (!assigned)
                            throw new InvalidOperationException($"Doctor id={docId} no está asignado al departamento {consulta.DepartamentoId}.");
                    }
                    consulta.Doctores.Add(doc);
                }
            }

            await _context.SaveChangesAsync();

            // reload relations
            await _context.Entry(consulta).Reference(c => c.Paciente).LoadAsync();
            await _context.Entry(consulta).Reference(c => c.MedicoPrincipal).LoadAsync();
            await _context.Entry(consulta).Collection(c => c.Doctores).LoadAsync();
            if (consulta.DepartamentoId > 0) await _context.Entry(consulta).Reference(c => c.Departamento).LoadAsync();

            return _mapper.Map<ConsultaReadDto>(consulta);
        }

        public async Task<ConsultaReadDto> AddDiagnosticoAsync(int id, string diagnostico, int medicoId)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Doctores)
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null) throw new InvalidOperationException("Consulta no encontrada.");

            // Validaciones:
            // - If consulta is Pendiente or EnCurso -> cannot add diagnostico
            // - If consulta FechaConsulta in future -> Pendiente -> cannot
            var nowDate = DateTime.UtcNow.Date;
            if (consulta.FechaConsulta.Date > nowDate) throw new InvalidOperationException("No se puede añadir diagnóstico a una consulta pendiente (fecha futura).");
            if (consulta.Estado == "Pendiente" || consulta.Estado == "EnCurso")
                throw new InvalidOperationException("No se puede añadir diagnóstico mientras la consulta esté pendiente o en curso.");

            // medicoId debe existir y ser doctor
            var medico = await _context.Trabajadores.FindAsync(medicoId);
            if (medico == null) throw new InvalidOperationException("Médico que firma el diagnóstico no existe.");
            if (!string.Equals(medico.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo un doctor puede firmar el diagnóstico.");

            // If consulta already has diagnostico, overwrite? we allow overwrite — alternativa: throw.
            consulta.Diagnostico = diagnostico;
            consulta.Estado = "Finalizada";

            await _context.SaveChangesAsync();

            // reload relations
            await _context.Entry(consulta).Reference(c => c.Paciente).LoadAsync();
            await _context.Entry(consulta).Reference(c => c.MedicoPrincipal).LoadAsync();
            await _context.Entry(consulta).Collection(c => c.Doctores).LoadAsync();
            if (consulta.DepartamentoId > 0) await _context.Entry(consulta).Reference(c => c.Departamento).LoadAsync();

            return _mapper.Map<ConsultaReadDto>(consulta);
        }
    }
}
