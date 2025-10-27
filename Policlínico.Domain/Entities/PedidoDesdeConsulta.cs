using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("PedidosConsultas")]
    public class PedidoConsulta
    {
        [Key]
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Required]
        [Column("consulta_id")]
        public int ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }

        [Required]
        [Column("departamento_id")]
        public int DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; }

        [Column("fecha_pedido")]
        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

        public ICollection<PedidoConsultaDetalle>? Detalles { get; set; }
    }
}
