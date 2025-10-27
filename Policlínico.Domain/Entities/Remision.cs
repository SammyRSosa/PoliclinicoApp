using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    public abstract class Remision
    {
        [Key]
        [Column("id_remision")]
        public int IdRemision { get; set; }

        [Required]
        [Column("paciente_id")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Required]
        [Column("departamento_que_atiende_id")]
        public int DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; }

        [Required]
        [Column("fecha_consulta")]
        public DateTime FechaConsulta { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("tipo_remision")]
        public string Tipo { get; set; } = string.Empty;
    }
}
