using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("EntregaAConsultaDetalles")]
    public class EntregaAConsultaDetalle
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("entrega_consulta_id")]
        public int EntregaConsultaId { get; set; }

        [ForeignKey(nameof(EntregaConsultaId))]
        public EntregaAConsulta? EntregaConsulta { get; set; }

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
