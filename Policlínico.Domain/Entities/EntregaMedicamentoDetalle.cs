using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("EntregaMedicamentoDetalles")]
    public class EntregaMedicamentoDetalle
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("entrega_id")]
        public int EntregaId { get; set; }

        [ForeignKey(nameof(EntregaId))]
        public EntregaMedicamento? Entrega { get; set; }

        [Required]
        [Column("medicamento_id")]
        public int MedicamentoId { get; set; }

        [ForeignKey(nameof(MedicamentoId))]
        public Medicamento? Medicamento { get; set; }

        [Required]
        [Column("cantidad")]
        public int Cantidad { get; set; }
    }
}
