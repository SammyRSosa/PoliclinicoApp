using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class PedidoMedicamentoCreateDto
    {
        public int ConsultaId { get; set; }
        public DateTime FechaPedido { get; set; }
        public List<PedidoMedicamentoDetalleDto> Detalles { get; set; } = new();
    }

    public class PedidoMedicamentoDetalleDto
    {
        public int MedicamentoId { get; set; }
        public int Cantidad { get; set; }
    }
}
