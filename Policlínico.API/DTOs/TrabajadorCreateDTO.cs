namespace Policlínico.API.DTOs
{
    public class TrabajadorCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string EstadoLaboral { get; set; } = "Inactivo"; // "Activo" o "Inactivo"
        public List<int>? DepartamentosAsignados { get; set; } // puede estar vacío
    }
}
