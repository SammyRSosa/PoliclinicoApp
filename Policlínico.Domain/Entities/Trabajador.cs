using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("Trabajadores")]
    public class Trabajador
    {
        [Key]
        [Column("id_trabajador")]
        public int IdTrabajador { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("estado_laboral")]
        public string EstadoLaboral { get; set; } = "Activo";

        // Relaciones
        public ICollection<Asignacion>? Asignaciones { get; set; }
    }
}
