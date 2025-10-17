using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("Asignaciones")]
    public class Asignacion
    {
        [Key]
        [Column("id_asignacion")]
        public int IdAsignacion { get; set; }

        [Required]
        [Column("trabajador_id")]
        public int TrabajadorId { get; set; }

        [ForeignKey("TrabajadorId")]
        public Trabajador? Trabajador { get; set; }

        [Required]
        [Column("departamento_id")]
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public Departamento? Departamento { get; set; }

        [Required]
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }
    }
}
