using Microsoft.EntityFrameworkCore;
using Policlínico.Application.Interfaces;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.Application.Services
{
    public class PedidoConsultaService : IPedidoConsultaService
    {
        private readonly PoliclínicoDbContext _context;

        public PedidoConsultaService(PoliclínicoDbContext context)
        {
            _context = context;
        }

        public async Task<PedidoConsulta> CrearPedidoAsync(PedidoConsulta pedido)
        {
            // Verificar que la consulta exista
            var consulta = await _context.Consultas
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.IdConsulta == pedido.ConsultaId);

            if (consulta == null)
                throw new InvalidOperationException("La consulta no existe.");

            // Verificar que el departamento tenga un stock
            var stock = await _context.Stocks
                .Include(s => s.StockMedicamentos)
                .FirstOrDefaultAsync(s => s.DepartamentoId == consulta.DepartamentoId);

            if (stock == null)
                throw new InvalidOperationException("El departamento no tiene un stock asociado.");

            // Validar y descontar medicamentos
            foreach (var detalle in pedido.Detalles)
            {
                var stockMedicamento = stock.StockMedicamentos
                    ?.FirstOrDefault(sm => sm.MedicamentoId == detalle.MedicamentoId);

                if (stockMedicamento == null)
                    throw new InvalidOperationException($"El medicamento con ID {detalle.MedicamentoId} no se encuentra en el stock.");

                if (stockMedicamento.CantidadDisponible < detalle.Cantidad)
                    throw new InvalidOperationException($"No hay suficiente cantidad disponible del medicamento {detalle.MedicamentoId}.");

                // Descontar del stock
                stockMedicamento.CantidadDisponible -= detalle.Cantidad;
            }

            // Guardar el pedido y el descuento
            _context.PedidosConsulta.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }
    }
}
