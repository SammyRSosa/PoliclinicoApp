namespace Policlínico.API.DTOs
{
    public class AsignacionDto
    {
        public int DepartamentoId { get; set; }
        public string DepartamentoNombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado => FechaFin == null ? "Activo" : "Finalizado";
    }
}
