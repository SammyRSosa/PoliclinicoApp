using System;
using System.Collections.Generic;

namespace Policlínico.Application.DTOs
{
    public class SolicitudReadDto
    {
        public int IdSolicitud { get; set; }
        public int DepartamentoId { get; set; }
        public string DepartamentoNombre { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? JefeDepartamentoId { get; set; }
        public List<SolicitudDetalleReadDto> Detalles { get; set; } = new();
    }

    public class SolicitudDetalleReadDto
    {
        public int MedicamentoId { get; set; }
        public string MedicamentoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
