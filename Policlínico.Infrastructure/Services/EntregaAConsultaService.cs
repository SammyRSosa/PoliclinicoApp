using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.Infrastructure.Services
{
    public class EntregaAConsultaService : IEntregaConsultaService
    {
        private readonly PoliclínicoDbContext _context;

        public EntregaAConsultaService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<EntregaAConsultaReadDto> CreateAsync(EntregaAConsultaCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var consulta = await _context.Consultas.FindAsync(dto.ConsultaId);
            if (consulta == null) throw new InvalidOperationException("Consulta no existe.");

            var entrega = new EntregaAConsulta
            {
                ConsultaId = dto.ConsultaId,
                FechaEntrega = dto.FechaEntrega,
                Estado = "Activo"
            };

            foreach (var d in dto.Detalles)
            {
                if (!await _context.Medicamentos.AnyAsync(m => m.Id == d.MedicamentoId))
                    throw new InvalidOperationException($"Medicamento id={d.MedicamentoId} no existe.");

                entrega.Detalles.Add(new EntregaAConsultaDetalle
                {
                    MedicamentoId = d.MedicamentoId,
                    Cantidad = d.Cantidad
                });
            }

            _context.EntregasAConsulta.Add(entrega);
            await _context.SaveChangesAsync();

            return await MapToReadDtoAsync(entrega.IdEntregaConsulta);
        }

        public async Task<IEnumerable<EntregaAConsultaReadDto>> GetAllAsync()
        {
            var list = await _context.EntregasAConsulta
                .Include(e => e.Consulta)
                .Include(e => e.Detalles).ThenInclude(d => d.Medicamento)
                .ToListAsync();

            return list.Select(MapToReadDto).ToList();
        }

        public async Task<EntregaAConsultaReadDto?> GetByIdAsync(int id)
        {
            var e = await _context.EntregasAConsulta
                .Include(x => x.Consulta)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstOrDefaultAsync(x => x.IdEntregaConsulta == id);

            return e == null ? null : MapToReadDto(e);
        }

        private EntregaAConsultaReadDto MapToReadDto(EntregaAConsulta e)
        {
            return new EntregaAConsultaReadDto
            {
                IdEntregaConsulta = e.IdEntregaConsulta,
                ConsultaId = e.ConsultaId,
                ConsultaTipo = e.Consulta?.Tipo ?? string.Empty,
                FechaEntrega = e.FechaEntrega,
                Estado = e.Estado,
                Detalles = e.Detalles.Select(d => new EntregaAConsultaDetalleReadDto
                {
                    MedicamentoId = d.MedicamentoId,
                    MedicamentoNombre = d.Medicamento?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        private async Task<EntregaAConsultaReadDto> MapToReadDtoAsync(int id)
        {
            var e = await _context.EntregasAConsulta
                .Include(x => x.Consulta)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstAsync(x => x.IdEntregaConsulta == id);

            return MapToReadDto(e);
        }
    }
}
