namespace Policlínico.API.DTOs
{
    public class DepartamentoReadDto
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = "Inactivo";
        public string? JefeNombre { get; set; }
        public List<TrabajadorMiniDto> Trabajadores { get; set; } = new();
    }
}
