namespace Policlínico.API.DTOs
{
    public class DepartamentoUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int? JefeId { get; set; } // puede ponerse o quitarse
        public string Estado { get; set; } = "Inactivo"; // "Activo" o "Inactivo"
    }
}
