using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionDepartamentoController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;

        public AsignacionDepartamentoController(PoliclínicoDbContext context)
        {
            _context = context;
        }

        // GET: api/asignaciondepartamento
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var asignaciones = await _context.Asignaciones
                .Include(a => a.Trabajador)
                .Include(a => a.Departamento)
                .ToListAsync();

            return Ok(asignaciones);
        }

        // POST: api/asignaciondepartamento
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Asignacion asignacion)
        {
            var trabajador = await _context.Trabajadores
                .Include(t => t.Asignaciones)
                .FirstOrDefaultAsync(t => t.IdTrabajador == asignacion.TrabajadorId);

            if (trabajador == null)
                return BadRequest("El trabajador no existe.");

            var departamento = await _context.Departamentos.FindAsync(asignacion.DepartamentoId);
            if (departamento == null)
                return BadRequest("El departamento no existe.");

            // Evitar duplicado activo
            bool yaAsignado = await _context.Asignaciones
                .AnyAsync(a => a.TrabajadorId == asignacion.TrabajadorId
                            && a.DepartamentoId == asignacion.DepartamentoId
                            && a.FechaFin == null);

            if (yaAsignado)
                return BadRequest("El trabajador ya tiene una asignación activa en este departamento.");

            _context.Asignaciones.Add(asignacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = asignacion.IdAsignacion }, asignacion);
        }

        // PUT: api/asignaciondepartamento/finalizar/{id}
        [HttpPut("finalizar/{id}")]
        public async Task<IActionResult> Finalizar(int id)
        {
            var asignacion = await _context.Asignaciones
                .Include(a => a.Trabajador)
                .FirstOrDefaultAsync(a => a.IdAsignacion == id);

            if (asignacion == null)
                return NotFound("No se encontró la asignación.");

            // Verificar que no deje al trabajador sin departamentos activos
            bool unicoActivo = await _context.Asignaciones
                .CountAsync(a => a.TrabajadorId == asignacion.TrabajadorId && a.FechaFin == null) == 1;

            if (unicoActivo)
                asignacion.Trabajador.EstadoLaboral = "Inactivo";
                _context.Trabajadores.Update(asignacion.Trabajador);
                await _context.SaveChangesAsync();

            asignacion.FechaFin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(asignacion);
        }
    }
}
