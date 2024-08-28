using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class AtivoAdicionado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animais_Granjas_GranjaId",
                table: "Animais");

            migrationBuilder.RenameColumn(
                name: "CNPJAgroindustria",
                table: "Agroindustrias",
                newName: "CNPJ");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Granjas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Avaliacoes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Animais",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "GranjaId",
                table: "Animais",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Animais",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Agroindustrias",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Agroindustrias",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Ativo", "CNPJ" },
                values: new object[] { true, "00000000000100" });

            migrationBuilder.UpdateData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Ativo", "CNPJ" },
                values: new object[] { true, "99999999000199" });

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "XDc8+K+qzKv9c8a9V+Pjnw==", "7dX8qmsq5MP9J5dCxY14UsoYjZY79J8X7O1k0reN8ro=" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "CPF",
                value: "00000000000");

            migrationBuilder.AddForeignKey(
                name: "FK_Animais_Granjas_GranjaId",
                table: "Animais",
                column: "GranjaId",
                principalTable: "Granjas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animais_Granjas_GranjaId",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Granjas");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Animais");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Agroindustrias");

            migrationBuilder.RenameColumn(
                name: "CNPJ",
                table: "Agroindustrias",
                newName: "CNPJAgroindustria");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Animais",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GranjaId",
                table: "Animais",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Agroindustrias",
                keyColumn: "Id",
                keyValue: 1,
                column: "CNPJAgroindustria",
                value: "00.000.000/0001-00");

            migrationBuilder.UpdateData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CNPJ",
                value: "99.999.999/0001-99");

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "KGa6FDnQ8P0W8LHJ0s2gtg==", "eDFNP6sDC4q1YRlMK3Ie3IWOoRp7VaUOSNrR0e7hG4s=" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "CPF",
                value: "000.000.000-00");

            migrationBuilder.AddForeignKey(
                name: "FK_Animais_Granjas_GranjaId",
                table: "Animais",
                column: "GranjaId",
                principalTable: "Granjas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
