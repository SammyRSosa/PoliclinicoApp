using Microsoft.EntityFrameworkCore;
using Policlínico.Domain.Entities;

namespace Policlínico.Infrastructure.Data
{
    public class PoliclínicoDbContext : DbContext
    {
        public PoliclínicoDbContext(DbContextOptions<PoliclínicoDbContext> options)
            : base(options) { }

        // ============================
        // DbSets (Tablas)
        // ============================
        public DbSet<Paciente> Pacientes { get; set; } = null!;
        public DbSet<Departamento> Departamentos { get; set; } = null!;
        public DbSet<Trabajador> Trabajadores { get; set; } = null!;
        public DbSet<Asignacion> Asignaciones { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<Medicamento> Medicamentos { get; set; } = null!;
        public DbSet<StockMedicamento> StockMedicamentos { get; set; } = null!;
        public DbSet<SolicitudMedicamento> SolicitudesMedicamentos { get; set; } = null!;
        public DbSet<SolicitudMedicamentoDetalle> SolicitudMedicamentoDetalles { get; set; } = null!;
        public DbSet<EntregaMedicamento> EntregasMedicamentos { get; set; } = null!;
        public DbSet<EntregaMedicamentoDetalle> EntregaMedicamentoDetalles { get; set; } = null!;
        public DbSet<Remision> Remisiones { get; set; } = null!;
        public DbSet<RemisionExterna> RemisionesExternas { get; set; } = null!;
        public DbSet<RemisionInterna> RemisionesInternas { get; set; } = null!;
        public DbSet<Consulta> Consultas { get; set; } = null!;
        public DbSet<ConsultaProgramada> ConsultasProgramadas { get; set; } = null!;
        public DbSet<ConsultaEmergencia> ConsultasEmergencia { get; set; } = null!;
        public DbSet<PedidoConsulta> PedidosConsulta { get; set; } = null!;
        public DbSet<PedidoConsultaDetalle> PedidoConsultaDetalles { get; set; } = null!;

        // ============================
        // Modelado de Relaciones
        // ============================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Paciente ----------
            modelBuilder.Entity<Paciente>()
                .HasIndex(p => p.NumeroIdentidad)
                .IsUnique();

            // ---------- Departamento ----------
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Jefe)
                .WithMany()
                .HasForeignKey(d => d.JefeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Stock)
                .WithOne(s => s.Departamento)
                .HasForeignKey<Stock>(s => s.DepartamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Asignaciones ----------
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Trabajador)
                .WithMany(t => t.Asignaciones)
                .HasForeignKey(a => a.TrabajadorId);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Departamento)
                .WithMany(d => d.Asignaciones)
                .HasForeignKey(a => a.DepartamentoId);

            // ---------- Medicamentos / Stock ----------
            modelBuilder.Entity<StockMedicamento>()
                .HasKey(sm => sm.IdStockMedicamento);

            modelBuilder.Entity<StockMedicamento>()
                .HasOne(sm => sm.Stock)
                .WithMany(s => s.StockMedicamentos)
                .HasForeignKey(sm => sm.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockMedicamento>()
                .HasOne(sm => sm.Medicamento)
                .WithMany()
                .HasForeignKey(sm => sm.MedicamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Solicitudes / Entregas ----------
            modelBuilder.Entity<SolicitudMedicamento>()
                .HasMany(s => s.Detalles)
                .WithOne(d => d.Solicitud!)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EntregaMedicamento>()
                .HasMany(e => e.Detalles)
                .WithOne(d => d.Entrega!)
                .HasForeignKey(d => d.EntregaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Remisiones ----------
            modelBuilder.Entity<Remision>().UseTptMappingStrategy();
            modelBuilder.Entity<RemisionExterna>().ToTable("RemisionesExternas");
            modelBuilder.Entity<RemisionInterna>().ToTable("RemisionesInternas");

            // ---------- Consultas ----------
            modelBuilder.Entity<Consulta>().UseTptMappingStrategy();
            modelBuilder.Entity<ConsultaProgramada>().ToTable("ConsultasProgramadas");
            modelBuilder.Entity<ConsultaEmergencia>().ToTable("ConsultasEmergencia");

            // ---------- Pedidos de consulta ----------
            modelBuilder.Entity<PedidoConsulta>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido!)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
