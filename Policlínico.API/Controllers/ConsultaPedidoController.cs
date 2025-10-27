using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policlínico.Infrastructure.Data;
using Policlínico.Domain.Entities;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultaPedidoController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;

        public ConsultaPedidoController(PoliclínicoDbContext context)
        {
            _context = context;
        }

        // ========================================
        // POST: api/ConsultaPedido/{idConsulta}/pedido
        // ========================================
        [HttpPost("{idConsulta}/pedido")]
        public async Task<IActionResult> CrearPedido(int idConsulta, [FromBody] List<PedidoItemDto> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest("Debe indicar al menos un medicamento a solicitar.");

            var consulta = await _context.Set<Consulta>()
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.IdConsulta == idConsulta);

            if (consulta == null)
                return NotFound("Consulta no encontrada.");

            var stock = await _context.Stocks
                .Include(s => s.Departamento)
                .Include(s => s.StockMedicamentos!)
                    .ThenInclude(sm => sm.Medicamento)
                .FirstOrDefaultAsync(s => s.DepartamentoId == consulta.DepartamentoId);

            if (stock == null)
                return BadRequest("El departamento de la consulta no tiene stock asociado.");

            var pedido = new PedidoConsulta
            {
                ConsultaId = consulta.IdConsulta,
                DepartamentoId = consulta.DepartamentoId,
                FechaPedido = DateTime.UtcNow,
                Detalles = new List<PedidoConsultaDetalle>()
            };

            foreach (var item in items)
            {
                var stockMed = stock.StockMedicamentos!.FirstOrDefault(sm => sm.MedicamentoId == item.MedicamentoId);
                if (stockMed == null)
                    return BadRequest($"El medicamento con ID {item.MedicamentoId} no pertenece al stock de este departamento.");

                if (stockMed.CantidadDisponible < item.Cantidad)
                    return BadRequest($"Stock insuficiente de {stockMed.Medicamento?.Nombre}. Disponible: {stockMed.CantidadDisponible}");

                // Descontar stock
                stockMed.CantidadDisponible -= item.Cantidad;

                // Agregar detalle
                pedido.Detalles.Add(new PedidoConsultaDetalle
                {
                    MedicamentoId = stockMed.MedicamentoId,
                    Cantidad = item.Cantidad
                });
            }

            _context.PedidosConsulta.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Pedido generado exitosamente.",
                consulta = consulta.IdConsulta,
                departamento = consulta.Departamento?.Nombre,
                fecha = pedido.FechaPedido,
                detalles = items
            });
        }

        // ========================================
        // GET: api/ConsultaPedido/{idConsulta}
        // ========================================
        [HttpGet("{idConsulta}")]
        public async Task<IActionResult> ObtenerPedidosPorConsulta(int idConsulta)
        {
            var pedidos = await _context.PedidosConsulta
                .Include(p => p.Detalles!)
                    .ThenInclude(d => d.Medicamento)
                .Where(p => p.ConsultaId == idConsulta)
                .ToListAsync();

            if (!pedidos.Any())
                return NotFound("La consulta no tiene pedidos registrados.");

            return Ok(pedidos);
        }
    }

    public class PedidoItemDto
    {
        public int MedicamentoId { get; set; }
        public int Cantidad { get; set; }
    }
}
