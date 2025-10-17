using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policlínico.Infrastructure.Data;
using Policlínico.Domain.Entities;
using AutoMapper;
using Policlínico.API.DTOs;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrabajadorController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;
        private readonly IMapper _mapper;

        public TrabajadorController(PoliclínicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ✅ GET: api/trabajador
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trabajadores = await _context.Trabajadores
                .Include(t => t.Asignaciones!)
                    .ThenInclude(a => a.Departamento)
                .ToListAsync();

            var dtoList = _mapper.Map<List<TrabajadorReadDto>>(trabajadores);
            return Ok(dtoList);
        }

        // ✅ GET: api/trabajador/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trabajador = await _context.Trabajadores
                .Include(t => t.Asignaciones!)
                    .ThenInclude(a => a.Departamento)
                .FirstOrDefaultAsync(t => t.IdTrabajador == id);

            if (trabajador == null)
                return NotFound("No se encontró el trabajador.");

            var dto = _mapper.Map<TrabajadorReadDto>(trabajador);
            return Ok(dto);
        }

        // ✅ GET: api/trabajador/{id}/historial
        [HttpGet("{id}/historial")]
        public async Task<IActionResult> GetHistorial(int id)
        {
            var trabajador = await _context.Trabajadores
                .Include(t => t.Asignaciones!)
                    .ThenInclude(a => a.Departamento)
                .FirstOrDefaultAsync(t => t.IdTrabajador == id);

            if (trabajador == null)
                return NotFound("No se encontró el trabajador.");

            var historial = trabajador.Asignaciones?
                .OrderByDescending(a => a.FechaInicio)
                .Select(a => new
                {
                    Departamento = a.Departamento?.Nombre,
                    FechaInicio = a.FechaInicio,
                    FechaFin = a.FechaFin,
                    Estado = a.FechaFin == null ? "Activo" : "Finalizado"
                })
                .ToList();

            return Ok(new
            {
                Trabajador = trabajador.Nombre,
                EstadoLaboral = trabajador.EstadoLaboral,
                Historial = historial
            });
        }

        // ✅ POST: api/trabajador
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrabajadorCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Cargo))
                return BadRequest("El nombre y el cargo son obligatorios.");

            var trabajador = _mapper.Map<Trabajador>(dto);

            _context.Trabajadores.Add(trabajador);
            await _context.SaveChangesAsync();

            // Si el trabajador es activo y tiene departamentos asignados
            if (trabajador.EstadoLaboral == "Activo" && dto.DepartamentosAsignados != null)
            {
                foreach (var depId in dto.DepartamentosAsignados)
                {
                    var existe = await _context.Departamentos.AnyAsync(d => d.IdDepartamento == depId);
                    if (existe)
                    {
                        _context.Asignaciones.Add(new Asignacion
                        {
                            TrabajadorId = trabajador.IdTrabajador,
                            DepartamentoId = depId,
                            FechaInicio = DateTime.UtcNow
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            var result = _mapper.Map<TrabajadorReadDto>(trabajador);
            return CreatedAtAction(nameof(GetById), new { id = trabajador.IdTrabajador }, result);
        }

        // ✅ PUT: api/trabajador/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TrabajadorUpdateDto dto)
        {
            var trabajador = await _context.Trabajadores
                .Include(t => t.Asignaciones!)
                .FirstOrDefaultAsync(t => t.IdTrabajador == id);

            if (trabajador == null)
                return NotFound("No se encontró el trabajador.");

            _mapper.Map(dto, trabajador);

            var asignacionesActivas = trabajador.Asignaciones?
                .Where(a => a.FechaFin == null)
                .ToList() ?? new List<Asignacion>();

            // Si se inactiva el trabajador → cerrar asignaciones
            if (dto.EstadoLaboral != "Activo")
            {
                foreach (var asign in asignacionesActivas)
                    asign.FechaFin = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                var dtoResult = _mapper.Map<TrabajadorReadDto>(trabajador);
                return Ok(dtoResult);
            }

            // Validar departamentos si sigue activo
            if (dto.DepartamentosAsignados == null || !dto.DepartamentosAsignados.Any())
                return BadRequest("Un trabajador activo debe pertenecer a al menos un departamento.");

            var departamentosValidos = await _context.Departamentos
                .Where(d => dto.DepartamentosAsignados.Contains(d.IdDepartamento))
                .Select(d => d.IdDepartamento)
                .ToListAsync();

            if (departamentosValidos.Count != dto.DepartamentosAsignados.Count)
                return BadRequest("Uno o más departamentos no existen.");

            // Cerrar asignaciones que ya no están
            foreach (var asign in asignacionesActivas)
            {
                if (!dto.DepartamentosAsignados.Contains(asign.DepartamentoId))
                    asign.FechaFin = DateTime.UtcNow;
            }

            // Crear nuevas asignaciones
            var actualesIds = asignacionesActivas.Select(a => a.DepartamentoId).ToList();
            var nuevos = dto.DepartamentosAsignados.Except(actualesIds);

            foreach (var depId in nuevos)
            {
                _context.Asignaciones.Add(new Asignacion
                {
                    TrabajadorId = trabajador.IdTrabajador,
                    DepartamentoId = depId,
                    FechaInicio = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            var result = _mapper.Map<TrabajadorReadDto>(trabajador);
            return Ok(result);
        }

        // ✅ DELETE: api/trabajador/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador == null)
                return NotFound("No existe el trabajador.");

            _context.Trabajadores.Remove(trabajador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
