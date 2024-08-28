using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class AgroindustriaFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgroindustriaId",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                column: "AgroindustriaId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AgroindustriaId",
                table: "Usuarios",
                column: "AgroindustriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Agroindustrias_AgroindustriaId",
                table: "Usuarios",
                column: "AgroindustriaId",
                principalTable: "Agroindustrias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Agroindustrias_AgroindustriaId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AgroindustriaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AgroindustriaId",
                table: "Usuarios");

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "xG5uQnY/KXtXQXJY3J779Q==", "WRU3XScAPAlQZlCJzof8sSImisXrbZ8PWZ5dQ1C4PZk=" });
        }
    }
}
