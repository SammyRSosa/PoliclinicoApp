using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class EntregaMedicamentoReadDto
    {
        public int IdEntrega { get; set; }
        public int DepartamentoDestinoId { get; set; }
        public string DepartamentoDestinoNombre { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? JefeAlmacenId { get; set; }
        public List<EntregaMedicamentoDetalleReadDto> Detalles { get; set; } = new();
    }

    public class EntregaMedicamentoDetalleReadDto
    {
        public int MedicamentoId { get; set; }
        public string MedicamentoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
