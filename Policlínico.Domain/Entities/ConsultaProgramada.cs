using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("ConsultasProgramadas")]
    public class ConsultaProgramada : Consulta
    {
        [Required]
        [Column("remision_id")]
        public int RemisionId { get; set; }
        public Remision? Remision { get; set; }

    }
}
