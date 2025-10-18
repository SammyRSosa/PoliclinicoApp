using Microsoft.AspNetCore.Mvc;
using Policlínico.Application.DTOs;
using Policlínico.Application.Interfaces;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoMedicamentoController : ControllerBase
    {
        private readonly IPedidoMedicamentoService _service;

        public PedidoMedicamentoController(IPedidoMedicamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoMedicamentoReadDto>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoMedicamentoReadDto>> GetById(int id)
        {
            var pedido = await _service.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoMedicamentoReadDto>> Create(PedidoMedicamentoCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdPedido }, created);
        }
    }
}
