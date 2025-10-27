using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Consultasfix10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_paciente_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicamentos_Stocks_StockIdStock",
                table: "Medicamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Trabajadores_Consultas_ConsultaIdConsulta",
                table: "Trabajadores");

            migrationBuilder.DropTable(
                name: "ConsultaTrabajadores");

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "EntregaAConsultaDetalles");

            migrationBuilder.DropTable(
                name: "Inventarios");

            migrationBuilder.DropTable(
                name: "PedidoMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "PuestosMedicos");

            migrationBuilder.DropTable(
                name: "EntregasAConsulta");

            migrationBuilder.DropTable(
                name: "PedidosMedicamentos");

            migrationBuilder.DropIndex(
                name: "IX_Trabajadores_ConsultaIdConsulta",
                table: "Trabajadores");

            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_StockIdStock",
                table: "Medicamentos");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "ConsultaIdConsulta",
                table: "Trabajadores");

            migrationBuilder.DropColumn(
                name: "StockIdStock",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "fecha_creacion",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "puesto_medico_id",
                table: "Consultas");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "Consultas",
                newName: "tipo_consulta");

            migrationBuilder.RenameColumn(
                name: "paciente_id",
                table: "Consultas",
                newName: "medico_atendio_id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_paciente_id",
                table: "Consultas",
                newName: "IX_Consultas_medico_atendio_id");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "Consultas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_consulta",
                table: "Consultas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateTable(
                name: "ConsultasEmergencia",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "integer", nullable: false),
                    paciente_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultasEmergencia", x => x.id_consulta);
                    table.ForeignKey(
                        name: "FK_ConsultasEmergencia_Consultas_id_consulta",
                        column: x => x.id_consulta,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultasEmergencia_Pacientes_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "Pacientes",
                        principalColumn: "id_paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosMedicamentosConsultas",
                columns: table => new
                {
                    id_movimiento_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_movimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosMedicamentosConsultas", x => x.id_movimiento_consulta);
                    table.ForeignKey(
                        name: "FK_MovimientosMedicamentosConsultas_Consultas_consulta_id",
                        column: x => x.consulta_id,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosMedicamentosConsultas_Departamentos_departamento~",
                        column: x => x.departamento_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidosConsulta",
                columns: table => new
                {
                    id_pedido_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_pedido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    departamento_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosConsulta", x => x.id_pedido_consulta);
                    table.ForeignKey(
                        name: "FK_PedidosConsulta_Consultas_consulta_id",
                        column: x => x.consulta_id,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosConsulta_Departamentos_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Remisiones",
                columns: table => new
                {
                    id_remision = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    paciente_id = table.Column<int>(type: "integer", nullable: false),
                    departamento_que_atiende_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_consulta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tipo_remision = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remisiones", x => x.id_remision);
                    table.ForeignKey(
                        name: "FK_Remisiones_Departamentos_departamento_que_atiende_id",
                        column: x => x.departamento_que_atiende_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remisiones_Pacientes_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "Pacientes",
                        principalColumn: "id_paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMedicamentos",
                columns: table => new
                {
                    id_stock_medicamento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stock_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad_disponible = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMedicamentos", x => x.id_stock_medicamento);
                    table.ForeignKey(
                        name: "FK_StockMedicamentos_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMedicamentos_Stocks_stock_id",
                        column: x => x.stock_id,
                        principalTable: "Stocks",
                        principalColumn: "id_stock",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosMedicamentosConsultasDetalles",
                columns: table => new
                {
                    id_detalle_movimiento_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    movimiento_consulta_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosMedicamentosConsultasDetalles", x => x.id_detalle_movimiento_consulta);
                    table.ForeignKey(
                        name: "FK_MovimientosMedicamentosConsultasDetalles_Medicamentos_medic~",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosMedicamentosConsultasDetalles_MovimientosMedicam~",
                        column: x => x.movimiento_consulta_id,
                        principalTable: "MovimientosMedicamentosConsultas",
                        principalColumn: "id_movimiento_consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoConsultaDetalles",
                columns: table => new
                {
                    id_detalle_pedido_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedido_consulta_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoConsultaDetalles", x => x.id_detalle_pedido_consulta);
                    table.ForeignKey(
                        name: "FK_PedidoConsultaDetalles_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoConsultaDetalles_PedidosConsulta_pedido_consulta_id",
                        column: x => x.pedido_consulta_id,
                        principalTable: "PedidosConsulta",
                        principalColumn: "id_pedido_consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsultasProgramadas",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "integer", nullable: false),
                    remision_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultasProgramadas", x => x.id_consulta);
                    table.ForeignKey(
                        name: "FK_ConsultasProgramadas_Consultas_id_consulta",
                        column: x => x.id_consulta,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultasProgramadas_Remisiones_remision_id",
                        column: x => x.remision_id,
                        principalTable: "Remisiones",
                        principalColumn: "id_remision",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RemisionesExternas",
                columns: table => new
                {
                    id_remision = table.Column<int>(type: "integer", nullable: false),
                    motivo_externo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemisionesExternas", x => x.id_remision);
                    table.ForeignKey(
                        name: "FK_RemisionesExternas_Remisiones_id_remision",
                        column: x => x.id_remision,
                        principalTable: "Remisiones",
                        principalColumn: "id_remision",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RemisionesInternas",
                columns: table => new
                {
                    id_remision = table.Column<int>(type: "integer", nullable: false),
                    departamento_origen_id = table.Column<int>(type: "integer", nullable: false),
                    motivo_interno = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemisionesInternas", x => x.id_remision);
                    table.ForeignKey(
                        name: "FK_RemisionesInternas_Remisiones_id_remision",
                        column: x => x.id_remision,
                        principalTable: "Remisiones",
                        principalColumn: "id_remision",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultasEmergencia_paciente_id",
                table: "ConsultasEmergencia",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultasProgramadas_remision_id",
                table: "ConsultasProgramadas",
                column: "remision_id");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosMedicamentosConsultas_consulta_id",
                table: "MovimientosMedicamentosConsultas",
                column: "consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosMedicamentosConsultas_departamento_id",
                table: "MovimientosMedicamentosConsultas",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosMedicamentosConsultasDetalles_medicamento_id",
                table: "MovimientosMedicamentosConsultasDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosMedicamentosConsultasDetalles_movimiento_consult~",
                table: "MovimientosMedicamentosConsultasDetalles",
                column: "movimiento_consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsultaDetalles_medicamento_id",
                table: "PedidoConsultaDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsultaDetalles_pedido_consulta_id",
                table: "PedidoConsultaDetalles",
                column: "pedido_consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosConsulta_consulta_id",
                table: "PedidosConsulta",
                column: "consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosConsulta_departamento_id",
                table: "PedidosConsulta",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Remisiones_departamento_que_atiende_id",
                table: "Remisiones",
                column: "departamento_que_atiende_id");

            migrationBuilder.CreateIndex(
                name: "IX_Remisiones_paciente_id",
                table: "Remisiones",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_StockMedicamentos_medicamento_id",
                table: "StockMedicamentos",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_StockMedicamentos_stock_id",
                table: "StockMedicamentos",
                column: "stock_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Trabajadores_medico_atendio_id",
                table: "Consultas",
                column: "medico_atendio_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Trabajadores_medico_atendio_id",
                table: "Consultas");

            migrationBuilder.DropTable(
                name: "ConsultasEmergencia");

            migrationBuilder.DropTable(
                name: "ConsultasProgramadas");

            migrationBuilder.DropTable(
                name: "MovimientosMedicamentosConsultasDetalles");

            migrationBuilder.DropTable(
                name: "PedidoConsultaDetalles");

            migrationBuilder.DropTable(
                name: "RemisionesExternas");

            migrationBuilder.DropTable(
                name: "RemisionesInternas");

            migrationBuilder.DropTable(
                name: "StockMedicamentos");

            migrationBuilder.DropTable(
                name: "MovimientosMedicamentosConsultas");

            migrationBuilder.DropTable(
                name: "PedidosConsulta");

            migrationBuilder.DropTable(
                name: "Remisiones");

            migrationBuilder.RenameColumn(
                name: "tipo_consulta",
                table: "Consultas",
                newName: "tipo");

            migrationBuilder.RenameColumn(
                name: "medico_atendio_id",
                table: "Consultas",
                newName: "paciente_id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_medico_atendio_id",
                table: "Consultas",
                newName: "IX_Consultas_paciente_id");

            migrationBuilder.AddColumn<int>(
                name: "ConsultaIdConsulta",
                table: "Trabajadores",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockIdStock",
                table: "Medicamentos",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "Consultas",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "Consultas",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_creacion",
                table: "Consultas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "puesto_medico_id",
                table: "Consultas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConsultaTrabajadores",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    trabajador_id = table.Column<int>(type: "integer", nullable: false),
                    es_principal = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultaTrabajadores", x => x.id);
                    table.ForeignKey(
                        name: "FK_ConsultaTrabajadores_Consultas_consulta_id",
                        column: x => x.consulta_id,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultaTrabajadores_Trabajadores_trabajador_id",
                        column: x => x.trabajador_id,
                        principalTable: "Trabajadores",
                        principalColumn: "id_trabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doctores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntregasAConsulta",
                columns: table => new
                {
                    id_entrega_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_entrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregasAConsulta", x => x.id_entrega_consulta);
                    table.ForeignKey(
                        name: "FK_EntregasAConsulta_Consultas_consulta_id",
                        column: x => x.consulta_id,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidosMedicamentos",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_pedido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosMedicamentos", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK_PedidosMedicamentos_Consultas_consulta_id",
                        column: x => x.consulta_id,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuestosMedicos",
                columns: table => new
                {
                    id_puesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuestosMedicos", x => x.id_puesto);
                });

            migrationBuilder.CreateTable(
                name: "EntregaAConsultaDetalles",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entrega_consulta_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregaAConsultaDetalles", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_EntregaAConsultaDetalles_EntregasAConsulta_entrega_consulta~",
                        column: x => x.entrega_consulta_id,
                        principalTable: "EntregasAConsulta",
                        principalColumn: "id_entrega_consulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntregaAConsultaDetalles_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoMedicamentoDetalles",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    pedido_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoMedicamentoDetalles", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_PedidoMedicamentoDetalles_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoMedicamentoDetalles_PedidosMedicamentos_pedido_id",
                        column: x => x.pedido_id,
                        principalTable: "PedidosMedicamentos",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_ConsultaIdConsulta",
                table: "Trabajadores",
                column: "ConsultaIdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_StockIdStock",
                table: "Medicamentos",
                column: "StockIdStock");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_puesto_medico_id",
                table: "Consultas",
                column: "puesto_medico_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaTrabajadores_consulta_id",
                table: "ConsultaTrabajadores",
                column: "consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaTrabajadores_trabajador_id",
                table: "ConsultaTrabajadores",
                column: "trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaAConsultaDetalles_entrega_consulta_id",
                table: "EntregaAConsultaDetalles",
                column: "entrega_consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaAConsultaDetalles_medicamento_id",
                table: "EntregaAConsultaDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasAConsulta_consulta_id",
                table: "EntregasAConsulta",
                column: "consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoMedicamentoDetalles_medicamento_id",
                table: "PedidoMedicamentoDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoMedicamentoDetalles_pedido_id",
                table: "PedidoMedicamentoDetalles",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosMedicamentos_consulta_id",
                table: "PedidosMedicamentos",
                column: "consulta_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_puesto_medico_id",
                table: "Consultas",
                column: "puesto_medico_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_paciente_id",
                table: "Consultas",
                column: "paciente_id",
                principalTable: "Pacientes",
                principalColumn: "id_paciente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicamentos_Stocks_StockIdStock",
                table: "Medicamentos",
                column: "StockIdStock",
                principalTable: "Stocks",
                principalColumn: "id_stock");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajadores_Consultas_ConsultaIdConsulta",
                table: "Trabajadores",
                column: "ConsultaIdConsulta",
                principalTable: "Consultas",
                principalColumn: "id_consulta");
        }
    }
}
