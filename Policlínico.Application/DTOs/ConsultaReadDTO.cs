namespace Policlínico.Application.DTOs
{
    public class ConsultaReadDto
    {
        public int IdConsulta { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaConsulta { get; set; }
        public string? Diagnostico { get; set; }

        // Datos vinculados
        public string? Paciente { get; set; }
        public string? Departamento { get; set; }
        public string? MedicoPrincipal { get; set; }
        public string? MedicoAtendio { get; set; }
        public string? Remision { get; set; }
    }
}
