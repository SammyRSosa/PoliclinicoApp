
public class MovimientoMedicamentoDto
{
    public string TipoMovimiento { get; set; } = null!; // Solicitud o Entrega
    public DateTime Fecha { get; set; }
    public List<MedicamentoDetalleDto> Medicamentos { get; set; } = new();
}