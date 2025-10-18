namespace Policlínico.Application.DTOs
{
    public class ConsultaUpdateDto
    {
        public DateTime? FechaConsulta { get; set; }
        public string? Diagnostico { get; set; } // permitir actualizar (con reglas)
        public string? Estado { get; set; } // permitir override si es necesario (validated)
        public int? DoctorPrincipalId { get; set; } // cambiar principal si necesario
        public List<int>? DoctoresParticipantesIds { get; set; } // reemplaza lista
    }
}
