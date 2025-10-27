using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Policlínico.Application.Interfaces;

namespace Policlínico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoriaClinicaController : ControllerBase
    {
        private readonly IHistoriaClinicaService _historiaService;

        public HistoriaClinicaController(IHistoriaClinicaService historiaService)
        {
            _historiaService = historiaService;
        }

        [HttpGet("{pacienteId}")]
        public async Task<IActionResult> GetHistoriaClinica(int pacienteId)
        {
            var historia = await _historiaService.ObtenerHistoriaClinicaAsync(pacienteId);
            if (historia == null || !historia.Any())
                return NotFound($"No se encontró historia clínica para el paciente con ID {pacienteId}");

            return Ok(historia);
        }
    }
}
