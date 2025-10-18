using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("EntregasAConsulta")]
    public class EntregaAConsulta
    {
        [Key]
        [Column("id_entrega_consulta")]
        public int IdEntregaConsulta { get; set; }

        [Required]
        [Column("consulta_id")]
        public int ConsultaId { get; set; }

        [ForeignKey(nameof(ConsultaId))]
        public Consulta? Consulta { get; set; }

        [Required]
        [Column("fecha_entrega")]
        public DateTime FechaEntrega { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Activo";

        public ICollection<EntregaAConsultaDetalle> Detalles { get; set; } = new List<EntregaAConsultaDetalle>();
    }
}
