using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Policlínico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartamentoAndStockControllers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Departamentos");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Departamentos");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Departamentos");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Departamentos",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departamentos",
                newName: "id_departamento");

            migrationBuilder.AddColumn<int>(
                name: "StockIdStock",
                table: "Medicamentos",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "Departamentos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "jefe_id",
                table: "Departamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_StockIdStock",
                table: "Medicamentos",
                column: "StockIdStock");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos",
                column: "jefe_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_departamento_id",
                table: "Stocks",
                column: "departamento_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_Trabajadores_jefe_id",
                table: "Departamentos",
                column: "jefe_id",
                principalTable: "Trabajadores",
                principalColumn: "id_trabajador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicamentos_Stocks_StockIdStock",
                table: "Medicamentos",
                column: "StockIdStock",
                principalTable: "Stocks",
                principalColumn: "id_stock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_Trabajadores_jefe_id",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicamentos_Stocks_StockIdStock",
                table: "Medicamentos");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_StockIdStock",
                table: "Medicamentos");

            migrationBuilder.DropIndex(
                name: "IX_Departamentos_jefe_id",
                table: "Departamentos");

            migrationBuilder.DropColumn(
                name: "StockIdStock",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "jefe_id",
                table: "Departamentos");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Departamentos",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id_departamento",
                table: "Departamentos",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Departamentos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Departamentos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Departamentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Departamentos",
                type: "text",
                nullable: true);
        }
    }
}
