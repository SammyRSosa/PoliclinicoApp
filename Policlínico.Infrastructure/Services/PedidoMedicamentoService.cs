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
    public class PedidoMedicamentoService : IPedidoMedicamentoService
    {
        private readonly PoliclínicoDbContext _context;

        public PedidoMedicamentoService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<PedidoMedicamentoReadDto> CreateAsync(PedidoMedicamentoCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var consulta = await _context.Consultas.FindAsync(dto.ConsultaId);
            if (consulta == null) throw new InvalidOperationException("Consulta no existe.");

            var pedido = new PedidoMedicamento
            {
                ConsultaId = dto.ConsultaId,
                FechaPedido = dto.FechaPedido,
                Estado = "Activo"
            };

            foreach (var d in dto.Detalles)
            {
                if (!await _context.Medicamentos.AnyAsync(m => m.Id == d.MedicamentoId))
                    throw new InvalidOperationException($"Medicamento id={d.MedicamentoId} no existe.");

                pedido.Detalles.Add(new PedidoMedicamentoDetalle
                {
                    MedicamentoId = d.MedicamentoId,
                    Cantidad = d.Cantidad
                });
            }

            _context.PedidosMedicamentos.Add(pedido);
            await _context.SaveChangesAsync();

            return await MapToReadDtoAsync(pedido.IdPedido);
        }

        public async Task<IEnumerable<PedidoMedicamentoReadDto>> GetAllAsync()
        {
            var list = await _context.PedidosMedicamentos
                .Include(p => p.Consulta)
                .Include(p => p.Detalles).ThenInclude(d => d.Medicamento)
                .ToListAsync();

            return list.Select(MapToReadDto).ToList();
        }

        public async Task<PedidoMedicamentoReadDto?> GetByIdAsync(int id)
        {
            var p = await _context.PedidosMedicamentos
                .Include(x => x.Consulta)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstOrDefaultAsync(x => x.IdPedido == id);

            return p == null ? null : MapToReadDto(p);
        }

        private PedidoMedicamentoReadDto MapToReadDto(PedidoMedicamento p)
        {
            return new PedidoMedicamentoReadDto
            {
                IdPedido = p.IdPedido,
                ConsultaId = p.ConsultaId,
                ConsultaTipo = p.Consulta?.Tipo ?? string.Empty,
                FechaPedido = p.FechaPedido,
                Estado = p.Estado,
                Detalles = p.Detalles.Select(d => new PedidoMedicamentoDetalleReadDto
                {
                    MedicamentoId = d.MedicamentoId,
                    MedicamentoNombre = d.Medicamento?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        private async Task<PedidoMedicamentoReadDto> MapToReadDtoAsync(int id)
        {
            var p = await _context.PedidosMedicamentos
                .Include(x => x.Consulta)
                .Include(x => x.Detalles).ThenInclude(d => d.Medicamento)
                .FirstAsync(x => x.IdPedido == id);

            return MapToReadDto(p);
        }
    }
}
