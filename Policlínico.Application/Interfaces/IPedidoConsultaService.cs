using Policlínico.Domain.Entities;

namespace Policlínico.Application.Interfaces
{
    public interface IPedidoConsultaService
    {
        Task<PedidoConsulta> CrearPedidoAsync(PedidoConsulta pedido);
    }
}
