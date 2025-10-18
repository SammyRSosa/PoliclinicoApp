using System.Collections.Generic;
using System.Threading.Tasks;
using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface IPedidoMedicamentoService
    {
        Task<PedidoMedicamentoReadDto> CreateAsync(PedidoMedicamentoCreateDto dto);
        Task<IEnumerable<PedidoMedicamentoReadDto>> GetAllAsync();
        Task<PedidoMedicamentoReadDto?> GetByIdAsync(int id);
    }
}
