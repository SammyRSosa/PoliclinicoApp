using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Nuevalogicadepartamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignaciones_Departamento");

            migrationBuilder.DropIndex(
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos");

            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "Departamentos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

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
                    table.ForeignKey(
                        name: "FK_Asignaciones_Departamentos_departamento_id",
                        column: x => x.departamento_id,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asignaciones_Trabajadores_trabajador_id",
                        column: x => x.trabajador_id,
                        principalTable: "Trabajadores",
                        principalColumn: "id_trabajador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos",
                column: "jefe_id");

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_departamento_id",
                table: "Asignaciones",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_trabajador_id",
                table: "Asignaciones",
                column: "trabajador_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignaciones");

            migrationBuilder.DropIndex(
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "Departamentos");

            migrationBuilder.CreateTable(
                name: "Asignaciones_Departamento",
                columns: table => new
                {
                    id_asignacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_departamento = table.Column<int>(type: "integer", nullable: false),
                    id_trabajador = table.Column<int>(type: "integer", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaciones_Departamento", x => x.id_asignacion);
                    table.ForeignKey(
                        name: "FK_Asignaciones_Departamento_Departamentos_id_departamento",
                        column: x => x.id_departamento,
                        principalTable: "Departamentos",
                        principalColumn: "id_departamento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asignaciones_Departamento_Trabajadores_id_trabajador",
                        column: x => x.id_trabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "id_trabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos",
                column: "jefe_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_Departamento_id_departamento",
                table: "Asignaciones_Departamento",
                column: "id_departamento");

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_Departamento_id_trabajador_id_departamento_fec~",
                table: "Asignaciones_Departamento",
                columns: new[] { "id_trabajador", "id_departamento", "fecha_fin" },
                unique: true,
                filter: "\"fecha_fin\" IS NULL");
        }
    }
}
