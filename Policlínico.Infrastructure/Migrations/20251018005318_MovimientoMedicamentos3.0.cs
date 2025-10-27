using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovimientoMedicamentos30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    id_paciente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    numero_identidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    contacto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    edad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.id_paciente);
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
                name: "Asignaciones",
                columns: table => new
                {
                    id_asignacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trabajador_id = table.Column<int>(type: "integer", nullable: false),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaciones", x => x.id_asignacion);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    fecha_consulta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    diagnostico = table.Column<string>(type: "text", nullable: true),
                    paciente_id = table.Column<int>(type: "integer", nullable: false),
                    medico_principal_id = table.Column<int>(type: "integer", nullable: false),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    puesto_medico_id = table.Column<int>(type: "integer", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.id_consulta);
                    table.ForeignKey(
                        name: "FK_Consultas_Pacientes_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "Pacientes",
                        principalColumn: "id_paciente",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Trabajadores",
                columns: table => new
                {
                    id_trabajador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cargo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    estado_laboral = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConsultaIdConsulta = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.id_trabajador);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Consultas_ConsultaIdConsulta",
                        column: x => x.ConsultaIdConsulta,
                        principalTable: "Consultas",
                        principalColumn: "id_consulta");
                });

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
                name: "Departamentos",
                columns: table => new
                {
                    id_departamento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    jefe_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.id_departamento);
                    table.ForeignKey(
                        name: "FK_Departamentos_Trabajadores_jefe_id",
                        column: x => x.jefe_id,
                        principalTable: "Trabajadores",
                        principalColumn: "id_trabajador",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Stocks",
                columns: table => new
                {
                    id_stock = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departamento_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.id_stock);
                    table.ForeignKey(
                        name: "FK_Stocks_Departamentos_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    StockIdStock = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicamentos_Stocks_StockIdStock",
                        column: x => x.StockIdStock,
                        principalTable: "Stocks",
                        principalColumn: "id_stock");
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
                name: "IX_Asignaciones_departamento_id",
                table: "Asignaciones",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_trabajador_id",
                table: "Asignaciones",
                column: "trabajador_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_departamento_id",
                table: "Consultas",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_medico_principal_id",
                table: "Consultas",
                column: "medico_principal_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_paciente_id",
                table: "Consultas",
                column: "paciente_id");

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
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos",
                column: "jefe_id");

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
                name: "IX_Medicamentos_StockIdStock",
                table: "Medicamentos",
                column: "StockIdStock");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_numero_identidad",
                table: "Pacientes",
                column: "numero_identidad",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_departamento_id",
                table: "Stocks",
                column: "departamento_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_ConsultaIdConsulta",
                table: "Trabajadores",
                column: "ConsultaIdConsulta");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignaciones_Departamentos_departamento_id",
                table: "Asignaciones",
                column: "departamento_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignaciones_Trabajadores_trabajador_id",
                table: "Asignaciones",
                column: "trabajador_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_departamento_id",
                table: "Consultas",
                column: "departamento_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_puesto_medico_id",
                table: "Consultas",
                column: "puesto_medico_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Trabajadores_medico_principal_id",
                table: "Consultas",
                column: "medico_principal_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_departamento_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Trabajadores_medico_principal_id",
                table: "Consultas");

            migrationBuilder.DropTable(
                name: "Asignaciones");

            migrationBuilder.DropTable(
                name: "ConsultaTrabajadores");

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "EntregaAConsultaDetalles");

            migrationBuilder.DropTable(
                name: "EntregaMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "Inventarios");

            migrationBuilder.DropTable(
                name: "PedidoMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "PuestosMedicos");

            migrationBuilder.DropTable(
                name: "SolicitudMedicamentoDetalles");

            migrationBuilder.DropTable(
                name: "EntregasAConsulta");

            migrationBuilder.DropTable(
                name: "EntregasMedicamentos");

            migrationBuilder.DropTable(
                name: "PedidosMedicamentos");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropTable(
                name: "SolicitudesMedicamentos");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
