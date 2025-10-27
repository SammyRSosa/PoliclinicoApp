namespace Policlínico.Application.DTOs
{
    public class ConsultaCreateDto
    {
        public string TipoConsulta { get; set; } = string.Empty; // "Programada" o "Emergencia"
        public DateTime FechaConsulta { get; set; }

        public int DepartamentoId { get; set; } // Emergencia
        public int? RemisionId { get; set; }     // Programada

        public int? PacienteId { get; set; }     // Emergencia
        public int MedicoPrincipalId { get; set; }
        public int MedicoAtendioId { get; set; }

        public string? Diagnostico { get; set; }
    }
}
