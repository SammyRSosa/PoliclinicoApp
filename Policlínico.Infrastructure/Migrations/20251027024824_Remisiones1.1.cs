using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remisiones11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoConsultaDetalles_PedidosConsulta_pedido_consulta_id",
                table: "PedidoConsultaDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosConsulta_Consultas_consulta_id",
                table: "PedidosConsulta");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosConsulta_Departamentos_departamento_id",
                table: "PedidosConsulta");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMedicamentos_Medicamentos_medicamento_id",
                table: "StockMedicamentos");

            migrationBuilder.DropTable(
                name: "MovimientosMedicamentosConsultasDetalles");

            migrationBuilder.DropTable(
                name: "MovimientosMedicamentosConsultas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosConsulta",
                table: "PedidosConsulta");

            migrationBuilder.RenameTable(
                name: "PedidosConsulta",
                newName: "PedidosConsultas");

            migrationBuilder.RenameColumn(
                name: "id_detalle_pedido_consulta",
                table: "PedidoConsultaDetalles",
                newName: "id_detalle");

            migrationBuilder.RenameColumn(
                name: "pedido_consulta_id",
                table: "PedidoConsultaDetalles",
                newName: "pedido_id");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoConsultaDetalles_pedido_consulta_id",
                table: "PedidoConsultaDetalles",
                newName: "IX_PedidoConsultaDetalles_pedido_id");

            migrationBuilder.RenameColumn(
                name: "id_pedido_consulta",
                table: "PedidosConsultas",
                newName: "id_pedido");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosConsulta_departamento_id",
                table: "PedidosConsultas",
                newName: "IX_PedidosConsultas_departamento_id");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosConsulta_consulta_id",
                table: "PedidosConsultas",
                newName: "IX_PedidosConsultas_consulta_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosConsultas",
                table: "PedidosConsultas",
                column: "id_pedido");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoConsultaDetalles_PedidosConsultas_pedido_id",
                table: "PedidoConsultaDetalles",
                column: "pedido_id",
                principalTable: "PedidosConsultas",
                principalColumn: "id_pedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosConsultas_Consultas_consulta_id",
                table: "PedidosConsultas",
                column: "consulta_id",
                principalTable: "Consultas",
                principalColumn: "id_consulta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosConsultas_Departamentos_departamento_id",
                table: "PedidosConsultas",
                column: "departamento_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMedicamentos_Medicamentos_medicamento_id",
                table: "StockMedicamentos",
                column: "medicamento_id",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoConsultaDetalles_PedidosConsultas_pedido_id",
                table: "PedidoConsultaDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosConsultas_Consultas_consulta_id",
                table: "PedidosConsultas");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosConsultas_Departamentos_departamento_id",
                table: "PedidosConsultas");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMedicamentos_Medicamentos_medicamento_id",
                table: "StockMedicamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosConsultas",
                table: "PedidosConsultas");

            migrationBuilder.RenameTable(
                name: "PedidosConsultas",
                newName: "PedidosConsulta");

            migrationBuilder.RenameColumn(
                name: "id_detalle",
                table: "PedidoConsultaDetalles",
                newName: "id_detalle_pedido_consulta");

            migrationBuilder.RenameColumn(
                name: "pedido_id",
                table: "PedidoConsultaDetalles",
                newName: "pedido_consulta_id");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoConsultaDetalles_pedido_id",
                table: "PedidoConsultaDetalles",
                newName: "IX_PedidoConsultaDetalles_pedido_consulta_id");

            migrationBuilder.RenameColumn(
                name: "id_pedido",
                table: "PedidosConsulta",
                newName: "id_pedido_consulta");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosConsultas_departamento_id",
                table: "PedidosConsulta",
                newName: "IX_PedidosConsulta_departamento_id");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosConsultas_consulta_id",
                table: "PedidosConsulta",
                newName: "IX_PedidosConsulta_consulta_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosConsulta",
                table: "PedidosConsulta",
                column: "id_pedido_consulta");

            migrationBuilder.CreateTable(
                name: "MovimientosMedicamentosConsultas",
                columns: table => new
                {
                    id_movimiento_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    fecha_movimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "MovimientosMedicamentosConsultasDetalles",
                columns: table => new
                {
                    id_detalle_movimiento_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    movimiento_consulta_id = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoConsultaDetalles_PedidosConsulta_pedido_consulta_id",
                table: "PedidoConsultaDetalles",
                column: "pedido_consulta_id",
                principalTable: "PedidosConsulta",
                principalColumn: "id_pedido_consulta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosConsulta_Consultas_consulta_id",
                table: "PedidosConsulta",
                column: "consulta_id",
                principalTable: "Consultas",
                principalColumn: "id_consulta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosConsulta_Departamentos_departamento_id",
                table: "PedidosConsulta",
                column: "departamento_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMedicamentos_Medicamentos_medicamento_id",
                table: "StockMedicamentos",
                column: "medicamento_id",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
