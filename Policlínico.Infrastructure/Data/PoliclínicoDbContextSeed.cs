using Microsoft.EntityFrameworkCore;
using Policlínico.Domain.Entities;

namespace Policlínico.Infrastructure.Data
{
    public static class DbContextSeed
    {
        public static async Task SeedAsync(PoliclínicoDbContext context)
        {
            // 🔹 1. Limpiar las tablas (orden correcto para evitar FK)
            var truncateSql = @"
                TRUNCATE TABLE 
                    ""PedidoConsultaDetalles"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""PedidosConsultas"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""EntregaMedicamentoDetalles"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""EntregasMedicamentos"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""SolicitudMedicamentoDetalles"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""SolicitudesMedicamentos"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""StockMedicamentos"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Stocks"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""ConsultasEmergencia"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""ConsultasProgramadas"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Consultas"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""RemisionesExternas"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""RemisionesInternas"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Remisiones"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Asignaciones"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Trabajadores"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Pacientes"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Departamentos"" RESTART IDENTITY CASCADE;
                TRUNCATE TABLE 
                    ""Medicamentos"" RESTART IDENTITY CASCADE;
            ";
            await context.Database.ExecuteSqlRawAsync(truncateSql);

            // 🔹 2. Medicamentos
            var medicamentos = new List<Medicamento>
            {
                new Medicamento { Nombre = "Paracetamol", Descripcion = "Analgésico y antipirético" },
                new Medicamento { Nombre = "Amoxicilina", Descripcion = "Antibiótico de amplio espectro" },
                new Medicamento { Nombre = "Ibuprofeno", Descripcion = "Antiinflamatorio no esteroideo" },
                new Medicamento { Nombre = "Omeprazol", Descripcion = "Inhibidor de la bomba de protones" },
                new Medicamento { Nombre = "Loratadina", Descripcion = "Antihistamínico" }
            };
            await context.Medicamentos.AddRangeAsync(medicamentos);
            await context.SaveChangesAsync();

            // 🔹 3. Trabajadores
            var trabajadores = new List<Trabajador>
            {
                new Trabajador { Nombre = "Dr. Juan Pérez", Cargo = "Médico General" },
                new Trabajador { Nombre = "Dra. Ana Gómez", Cargo = "Pediatra" },
                new Trabajador { Nombre = "Dr. Carlos Ruiz", Cargo = "Cardiólogo" },
                new Trabajador { Nombre = "Lic. María López", Cargo = "Jefa de Almacén" },
                new Trabajador { Nombre = "Dr. Miguel Torres", Cargo = "Jefe Departamento" }
            };
            await context.Trabajadores.AddRangeAsync(trabajadores);
            await context.SaveChangesAsync();

            // 🔹 4. Departamentos
            var departamentos = new List<Departamento>
            {
                new Departamento { Nombre = "Pediatría", Estado = "Activo", JefeId = 2 },
                new Departamento { Nombre = "Cardiología", Estado = "Activo", JefeId = 3 },
                new Departamento { Nombre = "Farmacia", Estado = "Activo", JefeId = 4 }
            };
            await context.Departamentos.AddRangeAsync(departamentos);
            await context.SaveChangesAsync();

            // 🔹 5. Asignaciones
            var asignaciones = new List<Asignacion>
            {
                new Asignacion { TrabajadorId = 1, DepartamentoId = 1, FechaInicio = DateTime.UtcNow.AddMonths(-6) },
                new Asignacion { TrabajadorId = 2, DepartamentoId = 1, FechaInicio = DateTime.UtcNow.AddMonths(-4) },
                new Asignacion { TrabajadorId = 3, DepartamentoId = 2, FechaInicio = DateTime.UtcNow.AddMonths(-3) },
                new Asignacion { TrabajadorId = 5, DepartamentoId = 2, FechaInicio = DateTime.UtcNow.AddMonths(-1) }
            };
            await context.Asignaciones.AddRangeAsync(asignaciones);
            await context.SaveChangesAsync();

            // 🔹 6. Pacientes
            var pacientes = new List<Paciente>
            {
                new Paciente { Nombre = "Luis Fernández", Edad = 35, NumeroIdentidad = "123456789", Direccion = "Av. Siempre Viva 123" },
                new Paciente { Nombre = "Ana Rodríguez", Edad = 29, NumeroIdentidad = "987654321", Direccion = "Calle Luna 45" },
                new Paciente { Nombre = "Pedro Sánchez", Edad = 50, NumeroIdentidad = "456789123", Direccion = "Calle Sol 9" }
            };
            await context.Pacientes.AddRangeAsync(pacientes);
            await context.SaveChangesAsync();

            // 🔹 7. Stocks
            var stocks = new List<Stock>
            {
                new Stock { DepartamentoId = 3 },
                new Stock { DepartamentoId = 1 },
                new Stock { DepartamentoId = 2 }
            };
            await context.Stocks.AddRangeAsync(stocks);
            await context.SaveChangesAsync();

            // 🔹 8. StockMedicamentos
            var stockMedicamentos = new List<StockMedicamento>
            {
                new StockMedicamento { StockId = 1, MedicamentoId = 1, CantidadDisponible = 500 },
                new StockMedicamento { StockId = 1, MedicamentoId = 2, CantidadDisponible = 300 },
                new StockMedicamento { StockId = 2, MedicamentoId = 3, CantidadDisponible = 150 },
                new StockMedicamento { StockId = 3, MedicamentoId = 4, CantidadDisponible = 200 }
            };
            await context.StockMedicamentos.AddRangeAsync(stockMedicamentos);
            await context.SaveChangesAsync();

            // 🔹 9. Remisiones
            var remisiones = new List<Remision>
            {
                new RemisionInterna
                {
                    PacienteId = 1,
                    DepartamentoId = 2,
                    DepartamentoOrigenId = 1,
                    FechaConsulta = DateTime.UtcNow.AddDays(-10),
                    MotivoInterno = "Evaluación cardiológica",
                    Tipo = "Interna"
                },
                new RemisionExterna
                {
                    PacienteId = 2,
                    DepartamentoId = 1,
                    FechaConsulta = DateTime.UtcNow.AddDays(-5),
                    MotivoExterno = "Referencia externa pediátrica",
                    Tipo = "Externa"
                }
            };
            await context.Remisiones.AddRangeAsync(remisiones);
            await context.SaveChangesAsync();

            // 🔹 10. Consultas
            var consultas = new List<Consulta>
            {
                new ConsultaEmergencia
                {
                    PacienteId = 1,
                    DepartamentoId = 1,
                    MedicoPrincipalId = 1,
                    MedicoAtendioId = 2,
                    FechaConsulta = DateTime.UtcNow.AddDays(-2),
                    Estado = "Finalizada",
                    Diagnostico = "Fiebre alta",
                    Tipo = "Emergencia"
                },
                new ConsultaProgramada
                {
                    DepartamentoId = 2,
                    MedicoPrincipalId = 3,
                    MedicoAtendioId = 5,
                    RemisionId = 1,
                    FechaConsulta = DateTime.UtcNow.AddDays(-1),
                    Estado = "EnCurso",
                    Diagnostico = "Revisión cardiológica",
                    Tipo = "Programada"
                }
            };
            await context.Consultas.AddRangeAsync(consultas);
            await context.SaveChangesAsync();

            // 🔹 11. Solicitudes y Entregas
            var solicitud = new SolicitudMedicamento
            {
                DepartamentoId = 1,
                FechaSolicitud = DateTime.UtcNow.AddDays(-3),
                Estado = "Aprobada",
                JefeDepartamentoId = 5
            };
            await context.SolicitudesMedicamentos.AddAsync(solicitud);
            await context.SaveChangesAsync();

            var solicitudDetalle = new SolicitudMedicamentoDetalle
            {
                SolicitudId = solicitud.IdSolicitud,
                MedicamentoId = 1,
                Cantidad = 50
            };
            await context.SolicitudMedicamentoDetalles.AddAsync(solicitudDetalle);
            await context.SaveChangesAsync();

            var entrega = new EntregaMedicamento
            {
                DepartamentoDestinoId = 1,
                FechaEntrega = DateTime.UtcNow.AddDays(-1),
                Estado = "Completada",
                JefeAlmacenId = 4
            };
            await context.EntregasMedicamentos.AddAsync(entrega);
            await context.SaveChangesAsync();

            var entregaDetalle = new EntregaMedicamentoDetalle
            {
                EntregaId = entrega.IdEntrega,
                MedicamentoId = 1,
                Cantidad = 50
            };
            await context.EntregaMedicamentoDetalles.AddAsync(entregaDetalle);
            await context.SaveChangesAsync();

            // 🔹 12. Pedidos de consulta
            var pedido = new PedidoConsulta
            {
                ConsultaId = 1,
                DepartamentoId = 3,
                FechaPedido = DateTime.UtcNow
            };
            await context.PedidosConsulta.AddAsync(pedido);
            await context.SaveChangesAsync();

            var pedidoDetalle = new PedidoConsultaDetalle
            {
                PedidoId = pedido.IdPedido,
                MedicamentoId = 2,
                Cantidad = 20
            };
            await context.PedidoConsultaDetalles.AddAsync(pedidoDetalle);
            await context.SaveChangesAsync();
        }
    }
}
