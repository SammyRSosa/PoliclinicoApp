using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("RemisionesExternas")]
    public class RemisionExterna : Remision
    {
        [Column("motivo_externo")]
        public string? MotivoExterno { get; set; }
    }
}
