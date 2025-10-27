using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.Application.Services
{
    public class HistoriaClinicaService : IHistoriaClinicaService
    {
        private readonly PoliclínicoDbContext _context;

        public HistoriaClinicaService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoriaClinicaDto>> ObtenerHistoriaClinicaAsync(int pacienteId)
        {
            // Consultas de emergencia
            var emergencias = await _context.ConsultasEmergencia
                .Where(c => c.PacienteId == pacienteId)
                .Include(c => c.Departamento)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.MedicoAtendio)
                .Select(c => new HistoriaClinicaDto
                {
                    ConsultaId = c.IdConsulta,
                    FechaConsulta = c.FechaConsulta,
                    TipoConsulta = c.Tipo,
                    Departamento = c.Departamento != null ? c.Departamento.Nombre : "",
                    MedicoPrincipal = c.MedicoPrincipal != null ? c.MedicoPrincipal.Nombre : "",
                    MedicoAtendio = c.MedicoAtendio != null ? c.MedicoAtendio.Nombre : "",
                    Diagnostico = c.Diagnostico
                })
                .ToListAsync();

            // Consultas programadas
            var programadas = await _context.ConsultasProgramadas
                .Where(c => c.Remision != null && c.Remision.PacienteId == pacienteId)
                .Include(c => c.Departamento)
                .Include(c => c.MedicoPrincipal)
                .Include(c => c.MedicoAtendio)
                .Include(c => c.Remision)
                .Select(c => new HistoriaClinicaDto
                {
                    ConsultaId = c.IdConsulta,
                    FechaConsulta = c.FechaConsulta,
                    TipoConsulta = c.Tipo,
                    Departamento = c.Departamento != null ? c.Departamento.Nombre : "",
                    MedicoPrincipal = c.MedicoPrincipal != null ? c.MedicoPrincipal.Nombre : "",
                    MedicoAtendio = c.MedicoAtendio != null ? c.MedicoAtendio.Nombre : "",
                    Diagnostico = c.Diagnostico
                })
                .ToListAsync();

            // Combinar todas las consultas y ordenar por fecha
            var historia = emergencias.Concat(programadas)
                                      .OrderByDescending(c => c.FechaConsulta)
                                      .ToList();

            return historia;
        }
    }
}
