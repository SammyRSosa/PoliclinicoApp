using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("PedidoConsultaDetalles")]
    public class PedidoConsultaDetalle
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("pedido_id")]
        public int PedidoId { get; set; }
        public PedidoConsulta? Pedido { get; set; }

        [Required]
        [Column("medicamento_id")]
        public int MedicamentoId { get; set; }
        public Medicamento? Medicamento { get; set; }

        [Required]
        [Column("cantidad")]
        public int Cantidad { get; set; }
    }
}
