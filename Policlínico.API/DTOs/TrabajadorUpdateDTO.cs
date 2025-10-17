namespace Policlínico.API.DTOs
{
    public class TrabajadorUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string EstadoLaboral { get; set; } = "Inactivo";
        public List<int> DepartamentosAsignados { get; set; } = new();
    }
}
