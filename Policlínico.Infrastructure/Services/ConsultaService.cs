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

        // -------------------------------
        // CREATE
        // -------------------------------
        public async Task<ConsultaReadDto> CreateAsync(ConsultaCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var nowDate = DateTime.UtcNow;

            // Validación de tipo
            if (string.IsNullOrWhiteSpace(dto.TipoConsulta))
                throw new InvalidOperationException("Debe indicar el tipo de consulta.");

            Consulta nuevaConsulta;

            // ===============================
            // CONSULTA PROGRAMADA
            // ===============================
            if (dto.TipoConsulta.Equals("Programada", StringComparison.OrdinalIgnoreCase))
            {
                var remision = await _context.Remisiones
                    .Include(r => r.Departamento)
                    .FirstOrDefaultAsync(r => r.IdRemision == dto.RemisionId)
                    ?? throw new InvalidOperationException("Remisión no encontrada.");

                var medicoPrincipal = await _context.Trabajadores.FindAsync(dto.MedicoPrincipalId)
                    ?? throw new InvalidOperationException("Médico principal no existe.");

                var medicoAtendio = await _context.Trabajadores.FindAsync(dto.MedicoAtendioId)
                    ?? throw new InvalidOperationException("Médico que atendió no existe.");

                var depExists = await _context.Departamentos.AnyAsync(d => d.IdDepartamento == remision.DepartamentoId);
                if (!depExists)
                    throw new InvalidOperationException("Departamento no válido para la remisión.");

                string estado = string.IsNullOrWhiteSpace(dto.Diagnostico)
                    ? "EnCurso"
                    : "Finalizada";

                nuevaConsulta = new ConsultaProgramada
                {
                    FechaConsulta = dto.FechaConsulta,
                    DepartamentoId = remision.DepartamentoId,
                    Estado = estado,
                    Diagnostico = dto.Diagnostico,
                    Tipo = "Programada",
                    RemisionId = (int)dto.RemisionId,
                    MedicoPrincipalId = dto.MedicoPrincipalId,
                    MedicoAtendioId = dto.MedicoAtendioId
                };

                _context.ConsultasProgramadas.Add((ConsultaProgramada)nuevaConsulta);
            }

            // ===============================
            // CONSULTA DE EMERGENCIA
            // ===============================
            else if (dto.TipoConsulta.Equals("Emergencia", StringComparison.OrdinalIgnoreCase))
            {
                var paciente = await _context.Pacientes.FindAsync(dto.PacienteId)
                    ?? throw new InvalidOperationException("Paciente no existe.");

                var medicoPrincipal = await _context.Trabajadores.FindAsync(dto.MedicoPrincipalId)
                    ?? throw new InvalidOperationException("Médico principal no existe.");

                var medicoAtendio = await _context.Trabajadores.FindAsync(dto.MedicoAtendioId)
                    ?? throw new InvalidOperationException("Médico que atendió no existe.");

                var depExists = await _context.Departamentos.AnyAsync(d => d.IdDepartamento == dto.DepartamentoId);
                if (!depExists)
                    throw new InvalidOperationException("Departamento no válido para consulta de emergencia.");

                string estado = string.IsNullOrWhiteSpace(dto.Diagnostico)
                    ? "EnCurso"
                    : "Finalizada";

                nuevaConsulta = new ConsultaEmergencia
                {
                    FechaConsulta = dto.FechaConsulta,
                    DepartamentoId = dto.DepartamentoId,
                    Estado = estado,
                    Diagnostico = dto.Diagnostico,
                    Tipo = "Emergencia",
                    PacienteId = (int)dto.PacienteId,
                    MedicoPrincipalId = dto.MedicoPrincipalId,
                    MedicoAtendioId = dto.MedicoAtendioId
                };

                _context.ConsultasEmergencia.Add((ConsultaEmergencia)nuevaConsulta);
            }
            else
            {
                throw new InvalidOperationException("Tipo de consulta no reconocido. Use 'Emergencia' o 'Programada'.");
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<ConsultaReadDto>(nuevaConsulta);
        }

        // -------------------------------
        // GET ALL
        // -------------------------------
        public async Task<IEnumerable<ConsultaReadDto>> GetAllAsync()
        {
            var consultasProgramadas = await _context.ConsultasProgramadas
                .Include(c => c.Remision)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.MedicoAtendio)
                .Include(c => c.Departamento)
                .ToListAsync();

            var consultasEmergencia = await _context.ConsultasEmergencia
                .Include(c => c.Paciente)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.MedicoAtendio)
                .Include(c => c.Departamento)
                .ToListAsync();

            var todas = new List<Consulta>();
            todas.AddRange(consultasProgramadas);
            todas.AddRange(consultasEmergencia);

            return _mapper.Map<IEnumerable<ConsultaReadDto>>(todas);
        }

        // -------------------------------
        // GET BY ID
        // -------------------------------
        public async Task<ConsultaReadDto?> GetByIdAsync(int id)
        {
            var consulta = await _context.ConsultasProgramadas
                .Include(c => c.Remision)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.MedicoAtendio)
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.IdConsulta == id)
                ?? (Consulta)await _context.ConsultasEmergencia
                .Include(c => c.Paciente)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.MedicoAtendio)
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null)
                return null;

            return _mapper.Map<ConsultaReadDto>(consulta);
        }

        // -------------------------------
        // UPDATE
        // -------------------------------
        public async Task<ConsultaReadDto> UpdateAsync(int id, ConsultaUpdateDto dto)
        {
            var consulta = await _context.ConsultasProgramadas
                .FirstOrDefaultAsync(c => c.IdConsulta == id)
                ?? (Consulta)await _context.ConsultasEmergencia
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null)
                throw new InvalidOperationException("Consulta no encontrada.");

            if (!string.IsNullOrWhiteSpace(dto.Diagnostico))
            {
                consulta.Diagnostico = dto.Diagnostico;
                consulta.Estado = "Finalizada";
            }

            if (dto.FechaConsulta.HasValue)
                consulta.FechaConsulta = dto.FechaConsulta.Value;

            await _context.SaveChangesAsync();
            return _mapper.Map<ConsultaReadDto>(consulta);
        }

        // -------------------------------
        // ADD DIAGNOSTICO
        // -------------------------------
        public async Task<ConsultaReadDto> AddDiagnosticoAsync(int id, string diagnostico, int medicoId)
        {
            var consulta = await _context.ConsultasProgramadas
                .FirstOrDefaultAsync(c => c.IdConsulta == id)
                ?? (Consulta)await _context.ConsultasEmergencia
                .FirstOrDefaultAsync(c => c.IdConsulta == id);

            if (consulta == null)
                throw new InvalidOperationException("Consulta no encontrada.");

            var medico = await _context.Trabajadores.FindAsync(medicoId)
                ?? throw new InvalidOperationException("Médico no existe.");

            if (!string.Equals(medico.Cargo, "Doctor", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo un doctor puede añadir diagnóstico.");

            consulta.Diagnostico = diagnostico;
            consulta.Estado = "Finalizada";

            await _context.SaveChangesAsync();
            return _mapper.Map<ConsultaReadDto>(consulta);
        }
    }
}
