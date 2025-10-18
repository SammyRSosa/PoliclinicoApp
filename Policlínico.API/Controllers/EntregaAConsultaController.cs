using Microsoft.AspNetCore.Mvc;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntregaAConsultaController : ControllerBase
    {
        private readonly IEntregaConsultaService _service;

        public EntregaAConsultaController(IEntregaConsultaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntregaAConsultaReadDto>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EntregaAConsultaReadDto>> GetById(int id)
        {
            var entrega = await _service.GetByIdAsync(id);
            if (entrega == null) return NotFound();
            return Ok(entrega);
        }

        [HttpPost]
        public async Task<ActionResult<EntregaAConsultaReadDto>> Create(EntregaAConsultaCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdEntregaConsulta }, created);
        }
    }
}
