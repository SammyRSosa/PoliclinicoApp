using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;

        public StockController(PoliclínicoDbContext context)
        {
            _context = context;
        }

        // GET: api/stock
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks
                .Include(s => s.Departamento)
                .Include(s => s.StockMedicamentos)
                .ToListAsync();

            return Ok(stocks);
        }

        // GET: api/stock/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _context.Stocks
                .Include(s => s.Departamento)
                .Include(s => s.StockMedicamentos)
                .FirstOrDefaultAsync(s => s.IdStock == id);

            if (stock == null)
                return NotFound($"No se encontró el stock con id {id}");

            return Ok(stock);
        }

        // POST: api/stock
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Stock stock)
        {
            var departamento = await _context.Departamentos.FindAsync(stock.DepartamentoId);
            if (departamento == null)
                return BadRequest("El departamento especificado no existe.");

            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = stock.IdStock }, stock);
        }

        // PUT: api/stock/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Stock stock)
        {
            if (id != stock.IdStock)
                return BadRequest("El ID del stock no coincide.");

            var existing = await _context.Stocks.FindAsync(id);
            if (existing == null)
                return NotFound($"No existe el stock con id {id}");

            existing.DepartamentoId = stock.DepartamentoId;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE: api/stock/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
                return NotFound($"No existe el stock con id {id}");

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
