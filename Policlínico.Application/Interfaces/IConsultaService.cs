using System.Collections.Generic;
using System.Threading.Tasks;
using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface IConsultaService
    {
        Task<ConsultaReadDto> CreateAsync(ConsultaCreateDto dto);
        Task<IEnumerable<ConsultaReadDto>> GetAllAsync();
        Task<ConsultaReadDto?> GetByIdAsync(int id);
        Task<ConsultaReadDto> UpdateAsync(int id, ConsultaUpdateDto dto);
        Task<ConsultaReadDto> AddDiagnosticoAsync(int id, string diagnostico, int medicoId); // medico who writes it (must be doctor)
    }
}
