using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class EntregaAConsultaCreateDto
    {
        public int ConsultaId { get; set; }
        public DateTime FechaEntrega { get; set; }
        public List<EntregaAConsultaDetalleDto> Detalles { get; set; } = new();
    }

    public class EntregaAConsultaDetalleDto
    {
        public int MedicamentoId { get; set; }
        public int Cantidad { get; set; }
    }
}
