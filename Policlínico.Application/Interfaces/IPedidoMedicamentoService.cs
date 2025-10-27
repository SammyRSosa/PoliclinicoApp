using Policlínico.Application.DTOs;

namespace Policlínico.Application.Interfaces
{
    public interface IPedidoMedicamentoService
    {
        Task<IEnumerable<PedidoMedicamentoReadDto>> GetAllAsync();
        Task<PedidoMedicamentoReadDto?> GetByIdAsync(int id);
        Task<PedidoMedicamentoReadDto> CreateAsync(PedidoMedicamentoCreateDto dto);
    }
}
