using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("SolicitudesMedicamentos")]
    public class SolicitudMedicamento
    {
        [Key]
        [Column("id_solicitud")]
        public int IdSolicitud { get; set; }

        [Required]
        [Column("departamento_id")]
        public int DepartamentoId { get; set; }

        [ForeignKey(nameof(DepartamentoId))]
        public Departamento? Departamento { get; set; }

        [Required]
        [Column("fecha_solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Inactivo"; // Inactivo -> cuando se crea

        [Column("jefe_departamento_id")]
        public int? JefeDepartamentoId { get; set; }

        [ForeignKey(nameof(JefeDepartamentoId))]
        public Trabajador? JefeDepartamento { get; set; }

        public ICollection<SolicitudMedicamentoDetalle> Detalles { get; set; } = new List<SolicitudMedicamentoDetalle>();
    }
}
