using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("PuestosMedicos")]
    public class PuestoMedico
    {
        [Key]
        [Column("id_puesto")]
        public int IdPuesto { get; set; }

        // Si quieres más campos luego (nombre, ubicación), puedes agregarlos.
        // Por ahora queda sólo el id como dijiste.
    }
}
