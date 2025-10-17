namespace Policlínico.API.DTOs
{
    public class TrabajadorReadDto
    {
        public int IdTrabajador { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string EstadoLaboral { get; set; } = "Inactivo";
        public List<AsignacionDto> Asignaciones { get; set; } = new();
    }
}
