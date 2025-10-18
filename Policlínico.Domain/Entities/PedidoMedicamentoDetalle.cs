using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("PedidoMedicamentoDetalles")]
    public class PedidoMedicamentoDetalle
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey(nameof(PedidoId))]
        public PedidoMedicamento? Pedido { get; set; }

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
