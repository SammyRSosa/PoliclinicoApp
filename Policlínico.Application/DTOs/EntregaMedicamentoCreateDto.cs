using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class EntregaMedicamentoCreateDto
    {
        public int DepartamentoDestinoId { get; set; }
        public DateTime FechaEntrega { get; set; }
        public List<EntregaMedicamentoDetalleDto> Detalles { get; set; } = new();
    }

    public class EntregaMedicamentoDetalleDto
    {
        public int MedicamentoId { get; set; }
        public int Cantidad { get; set; }
    }
}
