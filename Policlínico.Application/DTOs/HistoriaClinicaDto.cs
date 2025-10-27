using System;

namespace Policlínico.Application.DTOs
{
    public class HistoriaClinicaDto
    {
        public int ConsultaId { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string TipoConsulta { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string MedicoPrincipal { get; set; } = string.Empty;
        public string MedicoAtendio { get; set; } = string.Empty;
        public string? Diagnostico { get; set; }
    }
}
