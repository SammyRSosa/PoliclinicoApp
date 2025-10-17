using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("ConsultaTrabajadores")]
    public class ConsultaTrabajador
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("consulta_id")]
        public int ConsultaId { get; set; }
        [ForeignKey(nameof(ConsultaId))]
        public Consulta? Consulta { get; set; }

        [Required]
        [Column("trabajador_id")]
        public int TrabajadorId { get; set; }
        [ForeignKey(nameof(TrabajadorId))]
        public Trabajador? Trabajador { get; set; }

        [Column("es_principal")]
        public bool EsPrincipal { get; set; } = false;
    }
}
