using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("Pacientes")] // nombre de tabla en PostgreSQL
    public class Paciente
    {
        [Key] // Clave primaria
        [Column("id_paciente")]
        public int IdPaciente { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        [Column("direccion")]
        public string? Direccion { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("numero_identidad")]
        public string NumeroIdentidad { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("contacto")]
        public string? Contacto { get; set; }

        [Range(0, 120)]
        [Column("edad")]
        public int Edad { get; set; }

        // Relaciones (opcional)
        // public ICollection<Consulta>? Consultas { get; set; }
    }
}
