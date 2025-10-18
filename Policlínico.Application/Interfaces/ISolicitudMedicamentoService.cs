using System.Collections.Generic;
using System.Threading.Tasks;
using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface ISolicitudMedicamentoService
    {
        Task<SolicitudReadDto> CreateAsync(SolicitudCreateDto dto);
        Task<IEnumerable<SolicitudReadDto>> GetAllAsync();
        Task<SolicitudReadDto?> GetByIdAsync(int id);
        Task<SolicitudReadDto> ApproveAsync(int id, int jefeDepartamentoId);
    }
}
