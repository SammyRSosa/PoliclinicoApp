using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultasPuestosMedicos20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Consultas");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Consultas",
                newName: "diagnostico");

            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "Consultas",
                newName: "fecha_consulta");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Consultas",
                newName: "id_consulta");

            migrationBuilder.AddColumn<int>(
                name: "departamento_atiende_id",
                table: "Consultas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "departamento_origen_id",
                table: "Consultas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "doctor_principal_id",
                table: "Consultas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "Consultas",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "paciente_id",
                table: "Consultas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "puesto_medico_id",
                table: "Consultas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo",
                table: "Consultas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_departamento_atiende_id",
                table: "Consultas",
                column: "departamento_atiende_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_departamento_origen_id",
                table: "Consultas",
                column: "departamento_origen_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_doctor_principal_id",
                table: "Consultas",
                column: "doctor_principal_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_departamento_atiende_id",
                table: "Consultas",
                column: "departamento_atiende_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_departamento_origen_id",
                table: "Consultas",
                column: "departamento_origen_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_PuestosMedicos_puesto_medico_id",
                table: "Consultas",
                column: "puesto_medico_id",
                principalTable: "PuestosMedicos",
                principalColumn: "id_puesto");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Trabajadores_doctor_principal_id",
                table: "Consultas",
                column: "doctor_principal_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_departamento_atiende_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_departamento_origen_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_PuestosMedicos_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Trabajadores_doctor_principal_id",
                table: "Consultas");

            migrationBuilder.DropTable(
                name: "ConsultaTrabajadores");

            migrationBuilder.DropTable(
                name: "PuestosMedicos");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_departamento_atiende_id",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_departamento_origen_id",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_doctor_principal_id",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "departamento_atiende_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "departamento_origen_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "doctor_principal_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "paciente_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "puesto_medico_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "tipo",
                table: "Consultas");

            migrationBuilder.RenameColumn(
                name: "fecha_consulta",
                table: "Consultas",
                newName: "FechaNacimiento");

            migrationBuilder.RenameColumn(
                name: "diagnostico",
                table: "Consultas",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "id_consulta",
                table: "Consultas",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Consultas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Consultas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
