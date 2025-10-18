using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Policlínico.Domain.Entities;

namespace Policlínico.Infrastructure.Data
{
    public static class PoliclinicoDbContextSeed
    {
        public static async Task SeedAsync(PoliclínicoDbContext context)
        {
            if (context.Pacientes.Any()) return; // ya está poblada

            // === PACIENTES ===
            var pacientes = new List<Paciente>
            {
                new Paciente { Nombre = "Juan Pérez", NumeroIdentidad = "1001", Edad = 23 },
                new Paciente { Nombre = "María Gómez", NumeroIdentidad = "1002", Edad = 23}
            };
            context.Pacientes.AddRange(pacientes);

            // === TRABAJADORES ===
            var trabajadores = new List<Trabajador>
            {
                new Trabajador { Nombre = "Dr. Luis Hernández", Cargo = "Doctor" },
                new Trabajador { Nombre = "Dra. Ana Morales", Cargo = "Doctor" },
                new Trabajador { Nombre = "Carlos Díaz", Cargo = "Jefe de Almacén" },
                new Trabajador { Nombre = "Laura Torres", Cargo = "Enfermera" },
                new Trabajador { Nombre = "Pedro Ruiz", Cargo = "Doctor" }
            };
            context.Trabajadores.AddRange(trabajadores);
            await context.SaveChangesAsync();

            // === DEPARTAMENTOS ===
            var dep1 = new Departamento { Nombre = "Emergencias", JefeId = trabajadores[0].IdTrabajador, Estado = "Activo" };
            var dep2 = new Departamento { Nombre = "Pediatría", JefeId = trabajadores[1].IdTrabajador, Estado = "Activo" };
            var dep3 = new Departamento { Nombre = "Quemados", JefeId = null, Estado = "Inactivo" };
            context.Departamentos.AddRange(dep1, dep2, dep3);
            await context.SaveChangesAsync();

            // === PUESTOS MÉDICOS ===
            var puestos = new List<PuestoMedico>
            {
                new PuestoMedico { },
                new PuestoMedico { }
            };
            context.PuestosMedicos.AddRange(puestos);
            await context.SaveChangesAsync();

            // === ASIGNACIONES DE DOCTORES A DEPARTAMENTOS ===
            var asignaciones = new List<Asignacion>
            {
                new Asignacion { TrabajadorId = trabajadores[0].IdTrabajador, DepartamentoId = dep1.IdDepartamento, FechaInicio = DateTime.UtcNow.AddMonths(-6) },
                new Asignacion { TrabajadorId = trabajadores[1].IdTrabajador, DepartamentoId = dep2.IdDepartamento, FechaInicio = DateTime.UtcNow.AddMonths(-3) },
                new Asignacion { TrabajadorId = trabajadores[4].IdTrabajador, DepartamentoId = dep1.IdDepartamento, FechaInicio = DateTime.UtcNow.AddMonths(-1) }
            };
            context.Asignaciones.AddRange(asignaciones);

            // === MEDICAMENTOS ===
            var medicamentos = new List<Medicamento>
            {
                new Medicamento { Nombre = "Paracetamol", Descripcion = "Analgésico y antipirético" },
                new Medicamento { Nombre = "Amoxicilina", Descripcion = "Antibiótico de amplio espectro" },
                new Medicamento { Nombre = "Ibuprofeno", Descripcion = "Antiinflamatorio no esteroideo" }
            };
            context.Medicamentos.AddRange(medicamentos);
            await context.SaveChangesAsync();

            // === STOCKS POR DEPARTAMENTO ===
            var stocks = new List<Stock>
            {
                new Stock { DepartamentoId = dep1.IdDepartamento },
                new Stock { DepartamentoId = dep2.IdDepartamento }
            };
            context.Stocks.AddRange(stocks);
            await context.SaveChangesAsync();

            // === CONSULTAS ===
            var consulta1 = new Consulta
            {
                Tipo = "Programada Externa",
                PacienteId = pacientes[0].IdPaciente,
                MedicoPrincipalId = trabajadores[0].IdTrabajador,
                DepartamentoId = dep1.IdDepartamento,
                PuestoMedicoId = dep1.IdDepartamento,
                FechaConsulta = DateTime.UtcNow.AddDays(-1),
                Diagnostico = "Contusión leve tratada",
                Estado = "Finalizada",
                FechaCreacion = DateTime.UtcNow
            };
            var consulta2 = new Consulta
            {
                Tipo = "Urgencia",
                PacienteId = pacientes[1].IdPaciente,
                MedicoPrincipalId = trabajadores[1].IdTrabajador,
                DepartamentoId = dep2.IdDepartamento,
                FechaConsulta = DateTime.UtcNow,
                Estado = "EnCurso",
                FechaCreacion = DateTime.UtcNow
            };
            context.Consultas.AddRange(consulta1, consulta2);
            await context.SaveChangesAsync();

            // === CONSULTA - DOCTORES PARTICIPANTES ===
            var consultaTrabajadores = new List<ConsultaTrabajador>
            {
                new ConsultaTrabajador { ConsultaId = consulta1.IdConsulta, TrabajadorId = trabajadores[4].IdTrabajador },
                new ConsultaTrabajador { ConsultaId = consulta2.IdConsulta, TrabajadorId = trabajadores[0].IdTrabajador }
            };
            context.ConsultaTrabajadores.AddRange(consultaTrabajadores);

            // === SOLICITUD DE MEDICAMENTOS (DEPARTAMENTO -> ALMACÉN) ===
            var solicitud = new SolicitudMedicamento
            {
                DepartamentoId = dep1.IdDepartamento,
                FechaSolicitud = DateTime.UtcNow.AddDays(-2),
                Estado = "Inactiva"
            };
            context.SolicitudesMedicamentos.Add(solicitud);
            await context.SaveChangesAsync();

            var solicitudDetalles = new List<SolicitudMedicamentoDetalle>
            {
                new SolicitudMedicamentoDetalle { SolicitudId = solicitud.IdSolicitud, MedicamentoId = medicamentos[0].Id, Cantidad = 10 },
                new SolicitudMedicamentoDetalle { SolicitudId = solicitud.IdSolicitud, MedicamentoId = medicamentos[1].Id, Cantidad = 5 }
            };
            context.SolicitudMedicamentoDetalles.AddRange(solicitudDetalles);

            // === ENTREGA DE MEDICAMENTOS (ALMACÉN -> DEPARTAMENTO) ===
            var entrega = new EntregaMedicamento
            {
                DepartamentoDestinoId = dep1.IdDepartamento,
                FechaEntrega = DateTime.UtcNow.AddDays(-1),
                Estado = "Inactiva"
            };
            context.EntregasMedicamentos.Add(entrega);
            await context.SaveChangesAsync();

            var entregaDetalles = new List<EntregaMedicamentoDetalle>
            {
                new EntregaMedicamentoDetalle { EntregaId = entrega.IdEntrega, MedicamentoId = medicamentos[0].Id, Cantidad = 10 },
                new EntregaMedicamentoDetalle { EntregaId = entrega.IdEntrega, MedicamentoId = medicamentos[1].Id, Cantidad = 5 }
            };
            context.EntregaMedicamentoDetalles.AddRange(entregaDetalles);

            // === PEDIDO DE CONSULTA A ENFERMERÍA ===
            var pedido = new PedidoMedicamento
            {
                ConsultaId = consulta1.IdConsulta,
                FechaPedido = DateTime.UtcNow,
                Estado = "Inactiva"
            };
            context.PedidosMedicamentos.Add(pedido);
            await context.SaveChangesAsync();

            var pedidoDetalles = new List<PedidoMedicamentoDetalle>
            {
                new PedidoMedicamentoDetalle { PedidoId = pedido.IdPedido, MedicamentoId = medicamentos[2].Id, Cantidad = 2 }
            };
            context.PedidoMedicamentoDetalles.AddRange(pedidoDetalles);

            // === ENTREGA DE ENFERMERÍA A CONSULTA ===
            var entregaConsulta = new EntregaAConsulta
            {
                ConsultaId = consulta1.IdConsulta,
                FechaEntrega = DateTime.UtcNow,
                Estado = "Activa"
            };
            context.EntregasAConsulta.Add(entregaConsulta);
            await context.SaveChangesAsync();

            var entregaConsultaDetalles = new List<EntregaAConsultaDetalle>
            {
                new EntregaAConsultaDetalle { EntregaConsultaId = entregaConsulta.IdEntregaConsulta, MedicamentoId = medicamentos[2].Id, Cantidad = 2 }
            };
            context.EntregaAConsultaDetalles.AddRange(entregaConsultaDetalles);

            await context.SaveChangesAsync();
        }
    }
}
