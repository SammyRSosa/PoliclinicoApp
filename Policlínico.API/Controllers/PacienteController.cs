using Microsoft.AspNetCore.Mvc;
using Policlínico.Domain.Entities;
using Policlínico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PoliclínicoDbContext _context;

        public PacienteController(PoliclínicoDbContext context)
        {
            _context = context;
        }

        // GET api/paciente
        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return Ok(pacientes);
        }

        // POST api/paciente
        [HttpPost]
        public async Task<IActionResult> AddPaciente([FromBody] Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPacientes), new { id = paciente.IdPaciente }, paciente);
        }
    }
}
