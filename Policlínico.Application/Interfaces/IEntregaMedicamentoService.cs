using System.Collections.Generic;
using System.Threading.Tasks;
using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface IEntregaMedicamentoService
    {
        Task<EntregaMedicamentoReadDto> CreateAsync(EntregaMedicamentoCreateDto dto);
        Task<IEnumerable<EntregaMedicamentoReadDto>> GetAllAsync();
        Task<EntregaMedicamentoReadDto?> GetByIdAsync(int id);
        Task<EntregaMedicamentoReadDto> ApproveAsync(int id, int jefeAlmacenId);
    }
}
