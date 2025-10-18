using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultasPuestosMedicos60 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_PuestosMedicos_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Departamentos_puesto_medico_id",
                table: "Consultas",
                column: "puesto_medico_id",
                principalTable: "Departamentos",
                principalColumn: "id_departamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Departamentos_puesto_medico_id",
                table: "Consultas");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_PuestosMedicos_puesto_medico_id",
                table: "Consultas",
                column: "puesto_medico_id",
                principalTable: "PuestosMedicos",
                principalColumn: "id_puesto");
        }
    }
}
