using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("PedidosMedicamentos")]
    public class PedidoMedicamento
    {
        [Key]
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Required]
        [Column("consulta_id")]
        public int ConsultaId { get; set; }

        [ForeignKey(nameof(ConsultaId))]
        public Consulta? Consulta { get; set; }

        [Required]
        [Column("fecha_pedido")]
        public DateTime FechaPedido { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Activo"; // creado -> activo (enfermeria lo recibe)

        public ICollection<PedidoMedicamentoDetalle> Detalles { get; set; } = new List<PedidoMedicamentoDetalle>();
    }
}
