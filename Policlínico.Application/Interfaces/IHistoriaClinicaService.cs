using System.Collections.Generic;
using System.Threading.Tasks;
using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface IHistoriaClinicaService
    {
        Task<IEnumerable<HistoriaClinicaDto>> ObtenerHistoriaClinicaAsync(int pacienteId);
    }
}
