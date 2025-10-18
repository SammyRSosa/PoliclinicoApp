using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class EntregaAConsultaReadDto
    {
        public int IdEntregaConsulta { get; set; }
        public int ConsultaId { get; set; }
        public string ConsultaTipo { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<EntregaAConsultaDetalleReadDto> Detalles { get; set; } = new();
    }

    public class EntregaAConsultaDetalleReadDto
    {
        public int MedicamentoId { get; set; }
        public string MedicamentoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
