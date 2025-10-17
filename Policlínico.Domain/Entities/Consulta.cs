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
        [Column("tipo")]
        [MaxLength(50)]
        public string TipoConsulta { get; set; } = "Guardia"; // "Programada" o "Guardia"

        // Origen: puede ser un departamento interno (nullable) o un puesto medico externo (nullable)
        [Column("departamento_origen_id")]
        public int? DepartamentoOrigenId { get; set; }

        [ForeignKey(nameof(DepartamentoOrigenId))]
        public Departamento? DepartamentoOrigen { get; set; }

        [Column("puesto_medico_id")]
        public int? PuestoMedicoId { get; set; }

        [ForeignKey(nameof(PuestoMedicoId))]
        public PuestoMedico? PuestoMedico { get; set; }

        // Departamento que atiende (destino)
        [Required]
        [Column("departamento_atiende_id")]
        public int DepartamentoAtiendeId { get; set; }
        [ForeignKey(nameof(DepartamentoAtiendeId))]
        public Departamento DepartamentoAtiende { get; set; } = null!;

        // Paciente
        [Required]
        [Column("paciente_id")]
        public int PacienteId { get; set; }

        // Doctor principal (trabajador con cargo "Doctor")
        [Column("doctor_principal_id")]
        public int DoctorPrincipalId { get; set; }
        [ForeignKey(nameof(DoctorPrincipalId))]
        public Trabajador? DoctorPrincipal { get; set; }

        // Fecha de la consulta
        [Required]
        [Column("fecha_consulta")]
        public DateTime FechaConsulta { get; set; }

        // Diagnostico: resumen escrito por el doctor principal (nullable hasta que se guarde)
        [Column("diagnostico")]
        public string? Diagnostico { get; set; }

        // Estado: "Pendiente", "EnCurso", "Finalizada"
        [Required]
        [Column("estado")]
        [MaxLength(20)]
        public string Estado { get; set; } = "Pendiente";

        // Relación many-to-many a Trabajadores (doctores que participaron)
        public ICollection<ConsultaTrabajador>? ConsultaTrabajadores { get; set; }
    }
}
