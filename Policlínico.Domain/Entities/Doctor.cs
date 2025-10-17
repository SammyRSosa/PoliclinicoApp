namespace Policlínico.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
    public string? Telefono { get; set; }
}
