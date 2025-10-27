using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policlínico.Infrastructure.Data;
using Policlínico.Domain.Entities;
using Policlínico.Application.DTOs;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemisionController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;

        public RemisionController(PoliclínicoDbContext context)
        {
            _context = context;
        }

        // ✅ Obtener todas las remisiones (internas y externas)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var remisiones = await _context.Set<Remision>()
                .Include(r => r.Paciente)
                .Include(r => r.Departamento)
                .ToListAsync();

            return Ok(remisiones);
        }

        // ✅ Obtener remisión por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var remision = await _context.Set<Remision>()
                .Include(r => r.Paciente)
                .Include(r => r.Departamento)
                .FirstOrDefaultAsync(r => r.IdRemision == id);

            if (remision == null)
                return NotFound("Remisión no encontrada.");

            return Ok(remision);
        }

        // ✅ Crear nueva remisión (decide si es interna o externa)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RemisionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar tipo
            var tipo = dto.Tipo.Trim().ToLowerInvariant();
            if (tipo != "interna" && tipo != "externa")
                return BadRequest("El tipo de remisión debe ser 'Interna' o 'Externa'.");

            // Validar paciente
            var paciente = await _context.Pacientes.FindAsync(dto.PacienteId);
            if (paciente == null)
                return BadRequest("El paciente especificado no existe.");

            // Validar departamento que atiende
            var depAtiende = await _context.Departamentos.FindAsync(dto.DepartamentoAtiendeId);
            if (depAtiende == null)
                return BadRequest("El departamento que atiende no existe.");

            // Validar motivo
            if (string.IsNullOrWhiteSpace(dto.Motivo))
                return BadRequest("Debe especificar un motivo de remisión.");

            // Crear entidad dependiendo del tipo
            Remision remision;

            if (tipo == "interna")
            {
                if (!dto.DepartamentoOrigenId.HasValue)
                    return BadRequest("Las remisiones internas requieren un departamento de origen.");

                var depOrigen = await _context.Departamentos.FindAsync(dto.DepartamentoOrigenId.Value);
                if (depOrigen == null)
                    return BadRequest("El departamento de origen no existe.");

                remision = new RemisionInterna
                {
                    Tipo = "Interna",
                    PacienteId = dto.PacienteId,
                    DepartamentoId = dto.DepartamentoAtiendeId,
                    DepartamentoOrigenId = dto.DepartamentoOrigenId.Value,
                    MotivoInterno = dto.Motivo,
                    FechaConsulta = dto.FechaConsulta,
                    FechaCreacion = DateTime.UtcNow
                };
            }
            else // Externa
            {
                remision = new RemisionExterna
                {
                    Tipo = "Externa",
                    PacienteId = dto.PacienteId,
                    DepartamentoId = dto.DepartamentoAtiendeId,
                    MotivoExterno = dto.Motivo,
                    FechaConsulta = dto.FechaConsulta,
                    FechaCreacion = DateTime.UtcNow
                };
            }

            _context.Add(remision);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = remision.IdRemision }, remision);
        }

        // ✅ Actualizar remisión existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RemisionUpdateDto dto)
        {
            var remision = await _context.Set<Remision>()
                .FirstOrDefaultAsync(r => r.IdRemision == id);

            if (remision == null)
                return NotFound("Remisión no encontrada.");

            if (dto.FechaConsulta.HasValue)
                remision.FechaConsulta = dto.FechaConsulta.Value;

            if (!string.IsNullOrWhiteSpace(dto.Motivo))
            {
                if (remision is RemisionInterna interna)
                    interna.MotivoInterno = dto.Motivo;
                else if (remision is RemisionExterna externa)
                    externa.MotivoExterno = dto.Motivo;
            }

            await _context.SaveChangesAsync();
            return Ok(remision);
        }

        // ✅ Eliminar remisión
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var remision = await _context.Set<Remision>().FindAsync(id);
            if (remision == null)
                return NotFound("Remisión no encontrada.");

            _context.Remove(remision);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
