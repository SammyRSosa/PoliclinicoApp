using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Policlínico.Domain.Entities
{
    [Table("Consultas")]
    public class Consulta
    {
        [Key]
        [Column("id_consulta")]
        public int IdConsulta { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("tipo")]
        // "Programada Departamento" | "CuerpoDeGuardia" | "Programada Externa"
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [Column("fecha_consulta")]
        public DateTime FechaConsulta { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        // "Pendiente" | "EnCurso" | "Finalizada"
        public string Estado { get; set; } = "Pendiente";

        [Column("diagnostico")]
        public string? Diagnostico { get; set; } // nota: campo simple según lo pediste

        // Relaciones
        [Required]
        [Column("paciente_id")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Required]
        [Column("medico_principal_id")]
        public int MedicoPrincipalId { get; set; }
        public Trabajador? MedicoPrincipal { get; set; }

        // Solo obligatorio para Programada
        [Column("departamento_id")]
        public int DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; }

        // Solo obligatorio para Programada (puesto médico externo o interno)
        [Column("puesto_medico_id")]
        public int? PuestoMedicoId { get; set; }
        public Departamento? PuestoMedico { get; set; }

        // Doctores participantes (many-to-many). EF Core creará la tabla intermedia si es bidireccional;
        // si no la tiene, puedes crear explicitamente un entity join. Aquí mantenemos explicit collection.
        public ICollection<Trabajador> Doctores { get; set; } = new List<Trabajador>();

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<ConsultaTrabajador>? AsignacionesConsulta { get; set; }
    }
}
