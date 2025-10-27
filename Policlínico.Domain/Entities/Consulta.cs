using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    public abstract class Consulta
    {
        [Key]
        [Column("id_consulta")]
        public int IdConsulta { get; set; }

        [Required]
        [Column("fecha_consulta")]
        public DateTime FechaConsulta { get; set; }

        [Required]
        [Column("departamento_id")]
        public int DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; }

        [Required]
        [Column("medico_principal_id")]
        public int MedicoPrincipalId { get; set; }
        public Trabajador? MedicoPrincipal { get; set; }

        
        [Required]
        [Column("medico_atendio_id")]
        public int MedicoAtendioId { get; set; }
        public Trabajador? MedicoAtendio { get; set; }

        [Required]
        [Column("estado")]
        public string Estado { get; set; } = "EnCurso"; // valores: "EnCurso", "Finalizada"

        [Column("diagnostico")]
        public string? Diagnostico { get; set; }

        [Required]
        [Column("tipo_consulta")]
        public string Tipo { get; set; } = string.Empty;
    }
}
