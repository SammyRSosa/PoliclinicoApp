namespace Policlínico.API.DTOs
{
    public class DepartamentoSimpleDto
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? JefeNombre { get; set; }
    }
}
