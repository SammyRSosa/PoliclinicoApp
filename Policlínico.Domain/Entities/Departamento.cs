using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("Departamentos")]
    public class Departamento
    {
        [Key]
        [Column("id_departamento")]
        public int IdDepartamento { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Inactivo";


        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

    // JefeId is optional: a department can be created without a jefe
    [Column("jefe_id")]
    public int? JefeId { get; set; }

    [ForeignKey("JefeId")]
    public Trabajador? Jefe { get; set; }

        public Stock? Stock { get; set; }

        // Relaciones con asignaciones
        public ICollection<Asignacion>? Asignaciones { get; set; }
    }
}
