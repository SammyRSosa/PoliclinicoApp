using Policlínico.Application.DTOs;


namespace Policlínico.Application.Interfaces
{
    public interface IConsultaService
    {
        Task<ConsultaReadDTO> CreateAsync(ConsultaCreateDTO dto);
        Task<ConsultaReadDTO?> GetByIdAsync(int id);
        Task<List<ConsultaSimpleDTO>> GetAllSimpleAsync();
        Task<ConsultaReadDTO> UpdateAsync(int id, ConsultaUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
