using System.Collections.Generic;
using System.Threading.Tasks;
using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface IEntregaConsultaService
    {
        Task<EntregaAConsultaReadDto> CreateAsync(EntregaAConsultaCreateDto dto);
        Task<IEnumerable<EntregaAConsultaReadDto>> GetAllAsync();
        Task<EntregaAConsultaReadDto?> GetByIdAsync(int id);
    }
}
