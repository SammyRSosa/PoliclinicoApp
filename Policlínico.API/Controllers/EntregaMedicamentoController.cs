using Microsoft.AspNetCore.Mvc;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntregaMedicamentoController : ControllerBase
    {
        private readonly IEntregaMedicamentoService _service;

        public EntregaMedicamentoController(IEntregaMedicamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntregaMedicamentoReadDto>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EntregaMedicamentoReadDto>> GetById(int id)
        {
            var entrega = await _service.GetByIdAsync(id);
            if (entrega == null) return NotFound();
            return Ok(entrega);
        }

        [HttpPost]
        public async Task<ActionResult<EntregaMedicamentoReadDto>> Create(EntregaMedicamentoCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdEntrega }, created);
        }

        [HttpPut("{id:int}/aprobar")]
        public async Task<ActionResult<EntregaMedicamentoReadDto>> Aprobar(int id, [FromQuery] int jefeAlmacenId)
        {
            var entrega = await _service.ApproveAsync(id, jefeAlmacenId);
            return Ok(entrega);
        }
    }
}
