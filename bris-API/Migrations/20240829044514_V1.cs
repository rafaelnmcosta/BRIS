using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<float>(
                name: "ValorRegistrado",
                table: "Doses",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataRegistro",
                table: "Doses",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "PodePreencher",
                table: "Doses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ResultadoFinal",
                table: "Avaliacoes",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "ProximaDoseOrdem",
                table: "Avaliacoes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProximaDoseSemana",
                table: "Avaliacoes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "M2R6Jh49Y/IlXyHf8YOQ1Q==", "VDUCQfty979vhm3cQw0Uclmc7Sj0N/DXICvhpDwcuzM=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PodePreencher",
                table: "Doses");

            migrationBuilder.DropColumn(
                name: "ProximaDoseOrdem",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "ProximaDoseSemana",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<float>(
                name: "ValorRegistrado",
                table: "Doses",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataRegistro",
                table: "Doses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ResultadoFinal",
                table: "Avaliacoes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Avaliacoes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "XDc8+K+qzKv9c8a9V+Pjnw==", "7dX8qmsq5MP9J5dCxY14UsoYjZY79J8X7O1k0reN8ro=" });
        }
    }
}
