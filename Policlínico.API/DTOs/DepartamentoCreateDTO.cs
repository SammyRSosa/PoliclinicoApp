namespace Policlínico.API.DTOs
{
    public class DepartamentoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        // Make JefeId optional so a department can be created without a jefe
        public int? JefeId { get; set; }
    }
}
