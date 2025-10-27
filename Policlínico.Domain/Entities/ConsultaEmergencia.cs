using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("ConsultasEmergencia")]
    public class ConsultaEmergencia : Consulta
    {
        [Required]
        [Column("paciente_id")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
        
    }
}
