using Microsoft.AspNetCore.Mvc;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudMedicamentoController : ControllerBase
    {
        private readonly ISolicitudMedicamentoService _service;

        public SolicitudMedicamentoController(ISolicitudMedicamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SolicitudCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdSolicitud }, created);
        }

        [HttpPut("{id}/aprobar/{jefeId}")]
        public async Task<IActionResult> Aprobar(int id, int jefeId)
        {
            var updated = await _service.ApproveAsync(id, jefeId);
            return Ok(updated);
        }
    }
}
