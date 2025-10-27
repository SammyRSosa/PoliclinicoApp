namespace Policlínico.Application.DTOs
{
    public class RemisionUpdateDto
    {
        public DateTime? FechaConsulta { get; set; }

        public required int PacienteId { get; set; }  
        public int? DepartamentoAtiendeId { get; set; } // Emergencia
        public required int RemisionId { get; set; }     // Programada
        public string? Motivo { get; set; }
    }
}
