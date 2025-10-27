using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("StockMedicamentos")]
    public class StockMedicamento
    {
        [Key]
        [Column("id_stock_medicamento")]
        public int IdStockMedicamento { get; set; }

        [Required]
        [Column("stock_id")]
        public int StockId { get; set; }
        [ForeignKey("StockId")]
        public Stock Stock { get; set; }

        [Required]
        [Column("medicamento_id")]
        public int MedicamentoId { get; set; }
        [ForeignKey("MedicamentoId")]
        public Medicamento Medicamento { get; set; }

        [Required]
        [Column("cantidad_disponible")]
        public int CantidadDisponible { get; set; }
    }
}
