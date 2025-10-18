using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("EntregasMedicamentos")]
    public class EntregaMedicamento
    {
        [Key]
        [Column("id_entrega")]
        public int IdEntrega { get; set; }

        [Required]
        [Column("departamento_destino_id")]
        public int DepartamentoDestinoId { get; set; }

        [ForeignKey(nameof(DepartamentoDestinoId))]
        public Departamento? DepartamentoDestino { get; set; }

        [Required]
        [Column("fecha_entrega")]
        public DateTime FechaEntrega { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Inactivo";

        [Column("jefe_almacen_id")]
        public int? JefeAlmacenId { get; set; }

        [ForeignKey(nameof(JefeAlmacenId))]
        public Trabajador? JefeAlmacen { get; set; }

        public ICollection<EntregaMedicamentoDetalle> Detalles { get; set; } = new List<EntregaMedicamentoDetalle>();
    }
}
