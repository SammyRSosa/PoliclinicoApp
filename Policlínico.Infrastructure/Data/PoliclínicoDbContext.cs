using Microsoft.EntityFrameworkCore;
using Policlínico.Domain.Entities;

namespace Policlínico.Infrastructure.Data;

public class PoliclínicoDbContext : DbContext
{
    public PoliclínicoDbContext(DbContextOptions<PoliclínicoDbContext> options)
        : base(options) { }

    public DbSet<Paciente> Pacientes => Set<Paciente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ejemplo: asegurar que el número de identidad sea único
        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.NumeroIdentidad)
            .IsUnique();
// Relación 1:1 entre Departamento y Stock
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Stock)
                .WithOne(s => s.Departamento)
                .HasForeignKey<Stock>(s => s.DepartamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Jefe - Departamento (1:N)
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Jefe)
                .WithMany()
                .HasForeignKey(d => d.JefeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Asignación muchos a muchos (manual)
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Trabajador)
                .WithMany(t => t.Asignaciones)
                .HasForeignKey(a => a.TrabajadorId);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Departamento)
                .WithMany(d => d.Asignaciones)
                .HasForeignKey(a => a.DepartamentoId);


            modelBuilder.Entity<ConsultaTrabajador>()
                .HasOne(ct => ct.Consulta)
                .WithMany(c => c.ConsultaTrabajadores)
                .HasForeignKey(ct => ct.ConsultaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConsultaTrabajador>()
                .HasOne(ct => ct.Trabajador)
                .WithMany(t => t.AsignacionesConsulta) // add navigation in Trabajador if desired
                .HasForeignKey(ct => ct.TrabajadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Si quieres que PuestoMedico tenga tabla simple
            modelBuilder.Entity<PuestoMedico>().ToTable("PuestosMedicos");
    }
    public DbSet<Doctor> Doctores { get; set; } = null!;
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Medicamento> Medicamentos { get; set; } = null!;
    public DbSet<Inventario> Inventarios { get; set; } = null!;

    public DbSet<Trabajador> Trabajadores { get; set; }

    public DbSet<Asignacion> Asignaciones { get; set; }

    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<PuestoMedico> PuestosMedicos { get; set; }
    public DbSet<ConsultaTrabajador> ConsultaTrabajadores { get; set; }
}
