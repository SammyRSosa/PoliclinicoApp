using Microsoft.EntityFrameworkCore;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;
using Policlínico.Infrastructure.Data;
using Policlínico.Domain.Entities;

namespace Policlínico.Application.Services
{
    public class PedidoMedicamentoService : IPedidoMedicamentoService
    {
        private readonly PoliclínicoDbContext _context;

        public PedidoMedicamentoService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoMedicamentoReadDto>> GetAllAsync()
        {
            return await _context.PedidosConsulta
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Medicamento)
                .Select(p => new PedidoMedicamentoReadDto
                {
                    IdPedido = p.IdPedido,
                    FechaPedido = p.FechaPedido,
                    Detalles = p.Detalles.Select(d => new PedidoMedicamentoDetalleReadDto
                    {
                        MedicamentoId = d.MedicamentoId,
                        Cantidad = d.Cantidad
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<PedidoMedicamentoReadDto?> GetByIdAsync(int id)
        {
            var pedido = await _context.PedidosConsulta
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Medicamento)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return null;

            return new PedidoMedicamentoReadDto
            {
                IdPedido = pedido.IdPedido,
                FechaPedido = pedido.FechaPedido,
                Detalles = pedido.Detalles.Select(d => new PedidoMedicamentoDetalleReadDto
                {
                    MedicamentoId = d.MedicamentoId,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        public async Task<PedidoMedicamentoReadDto> CreateAsync(PedidoMedicamentoCreateDto dto)
        {
            var pedido = new PedidoConsulta
            {
                ConsultaId = dto.ConsultaId,
                FechaPedido = DateTime.UtcNow,
                Detalles = dto.Detalles.Select(d => new PedidoConsultaDetalle
                {
                    MedicamentoId = d.MedicamentoId,
                    Cantidad = d.Cantidad
                }).ToList()
            };

            _context.PedidosConsulta.Add(pedido);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(pedido.IdPedido) ?? throw new Exception("Error al crear pedido");
        }
    }
}
