using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("Stocks")]
    public class Stock
    {
        [Key]
        [Column("id_stock")]
        public int IdStock { get; set; }

        [Required]
        [Column("departamento_id")]
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public Departamento? Departamento { get; set; }

        public ICollection<Medicamento>? Medicamentos { get; set; }
    }
}
