using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovimientoMedicamentos10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntregasAConsulta",
                columns: table => new
                {
                    id_entrega_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_entrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                name: "EntregasMedicamentos",
                columns: table => new
                {
                    id_entrega = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departamento_destino_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_entrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    jefe_almacen_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregasMedicamentos", x => x.id_entrega);
                    table.ForeignKey(
                        name: "FK_EntregasMedicamentos_Departamentos_departamento_destino_id",
                        column: x => x.departamento_destino_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntregasMedicamentos_Trabajadores_jefe_almacen_id",
                        column: x => x.jefe_almacen_id,
                        principalTable: "Trabajadores",
                        principalColumn: "id_trabajador");
                });

            migrationBuilder.CreateTable(
                name: "PedidosMedicamentos",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consulta_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_pedido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                name: "SolicitudesMedicamentos",
                columns: table => new
                {
                    id_solicitud = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_solicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    jefe_departamento_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesMedicamentos", x => x.id_solicitud);
                    table.ForeignKey(
                        name: "FK_SolicitudesMedicamentos_Departamentos_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesMedicamentos_Trabajadores_jefe_departamento_id",
                        column: x => x.jefe_departamento_id,
                        principalTable: "Trabajadores",
                        principalColumn: "id_trabajador");
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
                name: "EntregaMedicamentoDetalles",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entrega_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregaMedicamentoDetalles", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_EntregaMedicamentoDetalles_EntregasMedicamentos_entrega_id",
                        column: x => x.entrega_id,
                        principalTable: "EntregasMedicamentos",
                        principalColumn: "id_entrega",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntregaMedicamentoDetalles_Medicamentos_medicamento_id",
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
                    pedido_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "SolicitudMedicamentoDetalles",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    solicitud_id = table.Column<int>(type: "integer", nullable: false),
                    medicamento_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudMedicamentoDetalles", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_SolicitudMedicamentoDetalles_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudMedicamentoDetalles_SolicitudesMedicamentos_solici~",
                        column: x => x.solicitud_id,
                        principalTable: "SolicitudesMedicamentos",
                        principalColumn: "id_solicitud",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntregaAConsultaDetalles_entrega_consulta_id",
                table: "EntregaAConsultaDetalles",
                column: "entrega_consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaAConsultaDetalles_medicamento_id",
                table: "EntregaAConsultaDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaMedicamentoDetalles_entrega_id",
                table: "EntregaMedicamentoDetalles",
                column: "entrega_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaMedicamentoDetalles_medicamento_id",
                table: "EntregaMedicamentoDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasAConsulta_consulta_id",
                table: "EntregasAConsulta",
                column: "consulta_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasMedicamentos_departamento_destino_id",
                table: "EntregasMedicamentos",
                column: "departamento_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasMedicamentos_jefe_almacen_id",
                table: "EntregasMedicamentos",
                column: "jefe_almacen_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMedicamentos_departamento_id",
                table: "SolicitudesMedicamentos",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMedicamentos_jefe_departamento_id",
                table: "SolicitudesMedicamentos",
                column: "jefe_departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudMedicamentoDetalles_medicamento_id",
                table: "SolicitudMedicamentoDetalles",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudMedicamentoDetalles_solicitud_id",
                table: "SolicitudMedicamentoDetalles",
                column: "solicitud_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntregaAConsultaDetalles");

            migrationBuilder.DropTable(
                name: "EntregaMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "PedidoMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "SolicitudMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "EntregasAConsulta");

            migrationBuilder.DropTable(
                name: "EntregasMedicamentos");

            migrationBuilder.DropTable(
                name: "PedidosMedicamentos");

            migrationBuilder.DropTable(
                name: "SolicitudesMedicamentos");
        }
    }
}
