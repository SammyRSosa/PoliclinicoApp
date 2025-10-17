using Policlínico.Application.DTOs;
using Policlínico.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Policlínico.Infrastructure.Data;
using Policlínico.Application.Interfaces;


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

        public async Task<ConsultaReadDTO> CreateAsync(ConsultaCreateDTO dto)
        {
            // Validaciones básicas
            var deptAtiende = await _context.Departamentos.FindAsync(dto.DepartamentoAtiendeId);
            if (deptAtiende == null) throw new InvalidOperationException("Departamento que atiende no existe.");

            if (dto.PuestoMedicoId.HasValue)
            {
                var puesto = await _context.PuestosMedicos.FindAsync(dto.PuestoMedicoId.Value);
                if (puesto == null) throw new InvalidOperationException("Puesto médico origen no existe.");
            }

            // Validar doctor principal existe y es doctor
            var doctorPrincipal = await _context.Trabajadores.FindAsync(dto.DoctorPrincipalId);
            if (doctorPrincipal == null) throw new InvalidOperationException("Doctor principal no existe.");
            if (!string.Equals(doctorPrincipal.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Doctor principal no tiene cargo 'Doctor'.");

            // Reglas de negocio sobre fecha y diagnóstico
            var now = DateTime.UtcNow;
            string estado;
            if (!string.IsNullOrWhiteSpace(dto.Diagnostico))
            {
                // No se permite diagnóstico si fecha futura
                if (dto.FechaConsulta > now)
                    throw new InvalidOperationException("No se puede agregar diagnóstico a una consulta con fecha futura.");
                estado = "Finalizada";
            }
            else
            {
                if (dto.FechaConsulta > now) estado = "Pendiente";
                else estado = "EnCurso";
            }

            var consulta = _mapper.Map<Consulta>(dto);
            consulta.Estado = estado;
            consulta.TipoConsulta = string.IsNullOrWhiteSpace(dto.TipoConsulta) ? "Guardia" : dto.TipoConsulta;

            // Guardar consulta
            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            // Manejar participantes: asegurarse de que existan y sean doctores
            var participantesIds = dto.DoctoresParticipantesIds ?? new List<int>();
            // asegurar que el principal esté en la lista
            if (!participantesIds.Contains(dto.DoctorPrincipalId))
                participantesIds.Add(dto.DoctorPrincipalId);

            var consultaTrabajadores = new List<ConsultaTrabajador>();
            foreach (var tId in participantesIds.Distinct())
            {
                var trabajador = await _context.Trabajadores.FindAsync(tId);
                if (trabajador == null) continue; // ignorar IDs inválidos
                if (!string.Equals(trabajador.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase)) 
                    continue; // ignorar si no es doctor

                consultaTrabajadores.Add(new ConsultaTrabajador
                {
                    ConsultaId = consulta.IdConsulta,
                    TrabajadorId = trabajador.IdTrabajador,
                    EsPrincipal = (tId == dto.DoctorPrincipalId)
                });
            }

            if (consultaTrabajadores.Any())
            {
                _context.ConsultaTrabajadores.AddRange(consultaTrabajadores);
                await _context.SaveChangesAsync();
            }

            // Si viene diagnostico, lo seteo
            if (!string.IsNullOrWhiteSpace(dto.Diagnostico))
            {
                consulta.Diagnostico = dto.Diagnostico;
                consulta.Estado = "Finalizada";
                await _context.SaveChangesAsync();
            }

            // recargar con navegación
            await _context.Entry(consulta).Reference(c => c.DoctorPrincipal).LoadAsync();
            await _context.Entry(consulta).Collection(c => c.ConsultaTrabajadores).LoadAsync();
            foreach (var ct in consulta.ConsultaTrabajadores ?? Enumerable.Empty<ConsultaTrabajador>())
                await _context.Entry(ct).Reference(x => x.Trabajador).LoadAsync();

            return _mapper.Map<ConsultaReadDTO>(consulta);
        }

        public async Task<ConsultaReadDTO?> GetByIdAsync(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.DoctorPrincipal)
                .Include(c => c.ConsultaTrabajadores)
                    .ThenInclude(ct => ct.Trabajador)
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null) return null;
            return _mapper.Map<ConsultaReadDTO>(consulta);
        }

        public async Task<List<ConsultaSimpleDTO>> GetAllSimpleAsync()
        {
            var consultas = await _context.Consultas
                .OrderByDescending(c => c.FechaConsulta)
                .ToListAsync();

            return _mapper.Map<List<ConsultaSimpleDTO>>(consultas);
        }

        public async Task<ConsultaReadDTO> UpdateAsync(int id, ConsultaUpdateDTO dto)
        {
            var consulta = await _context.Consultas
                .Include(c => c.ConsultaTrabajadores)
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null) throw new InvalidOperationException("Consulta no encontrada.");

            var now = DateTime.UtcNow;

            // Si intentan poner diagnóstico y la fecha es futura => error
            if (!string.IsNullOrWhiteSpace(dto.Diagnostico) && consulta.FechaConsulta > now)
                throw new InvalidOperationException("No se puede agregar diagnóstico a una consulta con fecha futura.");

            // Mapear campos permitidos
            if (dto.FechaConsulta.HasValue)
                consulta.FechaConsulta = dto.FechaConsulta.Value;

            if (!string.IsNullOrWhiteSpace(dto.Diagnostico))
            {
                consulta.Diagnostico = dto.Diagnostico;
                consulta.Estado = "Finalizada";
            }

            if (!string.IsNullOrWhiteSpace(dto.Estado))
                consulta.Estado = dto.Estado;

            // Cambiar doctor principal si se pide
            if (dto.DoctorPrincipalId.HasValue)
            {
                var doc = await _context.Trabajadores.FindAsync(dto.DoctorPrincipalId.Value);
                if (doc == null) throw new InvalidOperationException("Nuevo doctor principal no existe.");
                if (!string.Equals(doc.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Nuevo doctor principal no tiene cargo 'Doctor'.");
                consulta.DoctorPrincipalId = dto.DoctorPrincipalId.Value;

                // Asegurar que sea participante
                if (!(consulta.ConsultaTrabajadores?.Any(ct => ct.TrabajadorId == consulta.DoctorPrincipalId) ?? false))
                {
                    var ct = new ConsultaTrabajador
                    {
                        ConsultaId = consulta.IdConsulta,
                        TrabajadorId = consulta.DoctorPrincipalId,
                        EsPrincipal = true
                    };
                    _context.ConsultaTrabajadores.Add(ct);
                }
            }

            // Actualizar lista de participantes: reemplazo simple
            if (dto.DoctoresParticipantesIds != null)
            {
                // eliminar actuales no en la nueva lista
                var actuales = consulta.ConsultaTrabajadores ?? new List<ConsultaTrabajador>();
                var nuevosIds = dto.DoctoresParticipantesIds.Distinct().ToList();

                foreach (var actual in actuales.ToList())
                {
                    if (!nuevosIds.Contains(actual.TrabajadorId))
                        _context.ConsultaTrabajadores.Remove(actual);
                }

                // agregar los que faltan
                foreach (var nuevoId in nuevosIds)
                {
                    if (!actuales.Any(a => a.TrabajadorId == nuevoId))
                    {
                        var trabajador = await _context.Trabajadores.FindAsync(nuevoId);
                        if (trabajador == null) continue;
                        if (!string.Equals(trabajador.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase)) continue;

                        _context.ConsultaTrabajadores.Add(new ConsultaTrabajador
                        {
                            ConsultaId = consulta.IdConsulta,
                            TrabajadorId = nuevoId,
                            EsPrincipal = (nuevoId == consulta.DoctorPrincipalId)
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            await _context.Entry(consulta).Reference(c => c.DoctorPrincipal).LoadAsync();
            await _context.Entry(consulta).Collection(c => c.ConsultaTrabajadores).LoadAsync();
            foreach (var ct in consulta.ConsultaTrabajadores ?? Enumerable.Empty<ConsultaTrabajador>())
                await _context.Entry(ct).Reference(x => x.Trabajador).LoadAsync();

            return _mapper.Map<ConsultaReadDTO>(consulta);
        }

        public async Task DeleteAsync(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return;

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
        }
    }
}
