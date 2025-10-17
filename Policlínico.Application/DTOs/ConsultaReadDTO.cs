namespace Policlínico.Application.DTOs
{
    public class ConsultaReadDTO
    {
        public int IdConsulta { get; set; }
        public string TipoConsulta { get; set; } = string.Empty;
        public int? DepartamentoOrigenId { get; set; }
        public int? PuestoMedicoId { get; set; }
        public int DepartamentoAtiendeId { get; set; }
        public int PacienteId { get; set; }
        public int DoctorPrincipalId { get; set; }
        public string? DoctorPrincipalNombre { get; set; }
        public List<TrabajadorMiniDto>? DoctoresParticipantes { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string? Diagnostico { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
