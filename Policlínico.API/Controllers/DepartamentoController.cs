using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;
using Policlínico.API.DTOs;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;
        private readonly IMapper _mapper;

        public DepartamentoController(PoliclínicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ✅ GET: api/departamento
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departamentos = await _context.Departamentos
                .Include(d => d.Jefe)
                .Include(d => d.Stock)
                .Include(d => d.Asignaciones!)
                    .ThenInclude(a => a.Trabajador)
                .ToListAsync();

            var dtoList = _mapper.Map<List<DepartamentoSimpleDto>>(departamentos);
            return Ok(dtoList);
        }

        // ✅ GET: api/departamento/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var departamento = await _context.Departamentos
                .Include(d => d.Jefe)
                .Include(d => d.Asignaciones!)
                    .ThenInclude(a => a.Trabajador)
                .FirstOrDefaultAsync(d => d.IdDepartamento == id);

            if (departamento == null)
                return NotFound("Departamento no encontrado.");

            var dto = _mapper.Map<DepartamentoDto>(departamento);
            return Ok(dto);
        }

        
       [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartamentoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre del departamento es obligatorio.");

            // Estado depende del jefe
            var estado = dto.JefeId.HasValue && dto.JefeId > 0 ? "Activo" : "Inactivo";

            var departamento = new Departamento
            {
                Nombre = dto.Nombre,
                Estado = estado,
                JefeId = (dto.JefeId.HasValue && dto.JefeId > 0) ? dto.JefeId : null
            };

            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();

            // Crear stock asociado automáticamente
            var stock = new Stock
            {
                DepartamentoId = departamento.IdDepartamento
            };
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = departamento.IdDepartamento }, departamento);
        }



        // ✅ PUT: api/departamento/{id}/asignar-jefe/{jefeId}
        [HttpPut("{id}/asignar-jefe/{jefeId}")]
        public async Task<IActionResult> AsignarJefe(int id, int jefeId)
        {
            var departamento = await _context.Departamentos
                .Include(d => d.Asignaciones)
                .FirstOrDefaultAsync(d => d.IdDepartamento == id);

            if (departamento == null)
                return NotFound("Departamento no encontrado.");

            var trabajador = await _context.Trabajadores.FindAsync(jefeId);
            if (trabajador == null)
                return NotFound("Trabajador no encontrado.");

            var asignaciones = departamento.Asignaciones ?? new List<Asignacion>();
            var pertenece = asignaciones.Any(a => a.TrabajadorId == jefeId && a.FechaFin == null);

            if (!pertenece)
            {
                _context.Asignaciones.Add(new Asignacion
                {
                    TrabajadorId = jefeId,
                    DepartamentoId = id,
                    FechaInicio = DateTime.UtcNow
                });
            }

            departamento.JefeId = jefeId;
            departamento.Estado = "Activo";

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<DepartamentoSimpleDto>(departamento);
            return Ok(new
            {
                mensaje = "Jefe asignado correctamente y departamento activado.",
                departamento = dto
            });
        }

        // ✅ POST: api/departamento/{id}/agregar-trabajadores
        [HttpPost("{id}/agregar-trabajadores")]
        public async Task<IActionResult> AgregarTrabajadores(int id, [FromBody] List<int> trabajadoresIds)
        {
            if (trabajadoresIds == null || trabajadoresIds.Count == 0)
                return BadRequest("Debe especificar al menos un trabajador.");

            var departamento = await _context.Departamentos
                .Include(d => d.Asignaciones)
                .FirstOrDefaultAsync(d => d.IdDepartamento == id);

            if (departamento == null)
                return NotFound("Departamento no encontrado.");

            var trabajadoresExistentes = await _context.Trabajadores
                .Where(t => trabajadoresIds.Contains(t.IdTrabajador))
                .ToListAsync();

            if (trabajadoresExistentes.Count != trabajadoresIds.Count)
                return BadRequest("Uno o más trabajadores especificados no existen.");

            var nuevosAsignados = new List<Asignacion>();

            foreach (var trabajadorId in trabajadoresIds)
            {
                var yaAsignado = (departamento.Asignaciones ?? new List<Asignacion>())
                    .Any(a => a.TrabajadorId == trabajadorId && a.FechaFin == null);

                if (!yaAsignado)
                {
                    nuevosAsignados.Add(new Asignacion
                    {
                        TrabajadorId = trabajadorId,
                        DepartamentoId = id,
                        FechaInicio = DateTime.UtcNow
                    });
                }
            }

            if (nuevosAsignados.Count == 0)
                return BadRequest("Todos los trabajadores ya pertenecen al departamento.");

            _context.Asignaciones.AddRange(nuevosAsignados);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = $"Se agregaron {nuevosAsignados.Count} trabajadores al departamento '{departamento.Nombre}'.",
                estadoDepartamento = departamento.Estado
            });
        }

        // ✅ DELETE: api/departamento/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
                return NotFound("No existe el departamento.");

            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
