namespace Policlínico.Application.DTOs
{
    public class RemisionReadDto
    {
        public required string Tipo { get; set; } = string.Empty; // "Programada" o "Emergencia"
        public required DateTime FechaConsulta { get; set; }

        public required int DepartamentoAtiendeId { get; set; } // Emergencia

        public required int PacienteId { get; set; }     // Emergencia

        public int? DepartamentoOrigenId { get; set; }

        public required string Motivo { get; set; }
        
    }
}
