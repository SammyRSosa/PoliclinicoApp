namespace Policlínico.Application.DTOs
{
    public class ConsultaCreateDto
    {
        public string TipoConsulta { get; set; } = "Guardia"; // "Programada" o "Guardia"
        public int? PuestoMedicoId { get; set; } // opcional
        public int DepartamentoAtiendeId { get; set; } // requerido
        public int PacienteId { get; set; } // requerido
        public int DoctorPrincipalId { get; set; } // requerido (trabajador con cargo "Doctor")
        public List<int>? DoctoresParticipantesIds { get; set; } = new();
        public DateTime FechaConsulta { get; set; }
        public string? Diagnostico { get; set; } // opcional al crear
    }
}
