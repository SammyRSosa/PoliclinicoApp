using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class SolicitudCreateDto
    {
        public int DepartamentoId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public List<SolicitudDetalleDto> Detalles { get; set; } = new();
    }

    public class SolicitudDetalleDto
    {
        public int MedicamentoId { get; set; }
        public int Cantidad { get; set; }
    }
}
