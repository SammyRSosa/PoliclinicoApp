namespace Policlínico.Application.DTOs
{
    public class ConsultaSimpleDTO
    {
        public int IdConsulta { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int DepartamentoAtiendeId { get; set; }
    }
}
