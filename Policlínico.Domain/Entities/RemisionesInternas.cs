using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("RemisionesInternas")]
    public class RemisionInterna : Remision
    {
        [Column("departamento_origen_id")]
        public int DepartamentoOrigenId { get; set; }

        // Ejemplo: motivo de transferencia interna
        [Column("motivo_interno")]
        public string? MotivoInterno { get; set; }
    }
}
