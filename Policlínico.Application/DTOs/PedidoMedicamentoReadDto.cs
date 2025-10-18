using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class PedidoMedicamentoReadDto
    {
        public int IdPedido { get; set; }
        public int ConsultaId { get; set; }
        public string ConsultaTipo { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<PedidoMedicamentoDetalleReadDto> Detalles { get; set; } = new();
    }

    public class PedidoMedicamentoDetalleReadDto
    {
        public int MedicamentoId { get; set; }
        public string MedicamentoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
