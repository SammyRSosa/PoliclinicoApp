using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultasPuestosMedicos40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_departamento_atiende_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_departamento_origen_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Trabajadores_doctor_principal_id",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_departamento_origen_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "departamento_origen_id",
                table: "Consultas");

            migrationBuilder.RenameColumn(
                name: "doctor_principal_id",
                table: "Consultas",
                newName: "medico_principal_id");

            migrationBuilder.RenameColumn(
                name: "departamento_atiende_id",
                table: "Consultas",
                newName: "departamento_id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_doctor_principal_id",
                table: "Consultas",
                newName: "IX_Consultas_medico_principal_id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_departamento_atiende_id",
                table: "Consultas",
                newName: "IX_Consultas_departamento_id");

            migrationBuilder.AddColumn<int>(
                name: "ConsultaIdConsulta",
                table: "Trabajadores",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "Consultas",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_creacion",
                table: "Consultas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_ConsultaIdConsulta",
                table: "Trabajadores",
                column: "ConsultaIdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_paciente_id",
                table: "Consultas",
                column: "paciente_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_departamento_id",
                table: "Consultas",
                column: "departamento_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Pacientes_paciente_id",
                table: "Consultas",
                column: "paciente_id",
                principalTable: "Pacientes",
                principalColumn: "id_paciente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Trabajadores_medico_principal_id",
                table: "Consultas",
                column: "medico_principal_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajadores_Consultas_ConsultaIdConsulta",
                table: "Trabajadores",
                column: "ConsultaIdConsulta",
                principalTable: "Consultas",
                principalColumn: "id_consulta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_departamento_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Pacientes_paciente_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Trabajadores_medico_principal_id",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Trabajadores_Consultas_ConsultaIdConsulta",
                table: "Trabajadores");

            migrationBuilder.DropIndex(
                name: "IX_Trabajadores_ConsultaIdConsulta",
                table: "Trabajadores");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_paciente_id",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "ConsultaIdConsulta",
                table: "Trabajadores");

            migrationBuilder.DropColumn(
                name: "fecha_creacion",
                table: "Consultas");

            migrationBuilder.RenameColumn(
                name: "medico_principal_id",
                table: "Consultas",
                newName: "doctor_principal_id");

            migrationBuilder.RenameColumn(
                name: "departamento_id",
                table: "Consultas",
                newName: "departamento_atiende_id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_medico_principal_id",
                table: "Consultas",
                newName: "IX_Consultas_doctor_principal_id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_departamento_id",
                table: "Consultas",
                newName: "IX_Consultas_departamento_atiende_id");

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "Consultas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "departamento_origen_id",
                table: "Consultas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_departamento_origen_id",
                table: "Consultas",
                column: "departamento_origen_id");

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
                name: "FK_Consultas_Trabajadores_doctor_principal_id",
                table: "Consultas",
                column: "doctor_principal_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
