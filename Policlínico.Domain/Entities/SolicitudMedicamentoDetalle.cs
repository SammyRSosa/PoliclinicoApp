using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("SolicitudMedicamentoDetalles")]
    public class SolicitudMedicamentoDetalle
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("solicitud_id")]
        public int SolicitudId { get; set; }

        [ForeignKey(nameof(SolicitudId))]
        public SolicitudMedicamento? Solicitud { get; set; }

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
