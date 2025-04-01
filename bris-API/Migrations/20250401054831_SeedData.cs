using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Agroindustrias",
                columns: new[] { "Id", "Ativo", "CNPJ", "DataCadastro", "NomeFantasia", "RazaoSocial" },
                values: new object[,]
                {
                    { 1, true, "00000000000100", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(2364), "Agroindustria Default", "Agroindustria Default" },
                    { 2, true, "11111111000111", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(2371), "Agroindustria Nova", "Agroindustria Nova" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CPF", "DataCadastro", "Email", "Nome", "Telefone", "UltimoLogin" },
                values: new object[,]
                {
                    { 1, "00000000000", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4092), "admin@gmail.com", "Admin", null, null },
                    { 2, "11111111111", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4098), "gestor_granja@gmail.com", "Gestor Granja", null, null },
                    { 3, "22222222222", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4101), "gestor_agro@gmail.com", "Gestor Agro", null, null },
                    { 4, "33333333333", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4103), "tecnico@gmail.com", "Tecnico", null, null },
                    { 5, "44444444444", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4106), "visualizador@gmail.com", "Visualizador", null, null }
                });

            migrationBuilder.InsertData(
                table: "Granjas",
                columns: new[] { "Id", "AgroindustriaId", "Ativo", "CNPJ", "DataCadastro", "Endereco", "NomePropriedade" },
                values: new object[,]
                {
                    { 1, 1, true, "99999999000199", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(2427), "Rua teste 1", "Granja Teste 1" },
                    { 2, 1, true, "99999999000122", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(2434), "Rua teste 2", "Granja Teste 2" },
                    { 3, 2, true, "88888888000133", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(2438), "Rua nova 1", "Granja Nova 1" },
                    { 4, 2, true, "88888888000144", new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(2441), "Rua nova 2", "Granja Nova 2" }
                });

            migrationBuilder.InsertData(
                table: "Senhas",
                columns: new[] { "Id", "Salt", "SenhaHash", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "yOQWFM0EJZr+f/Gj9fqK4w==", "MzVSe/GhHMr3eggdjkPm0GDPTLy1C2xDOyZqlgVHc2Y=", 1 },
                    { 2, "yOQWFM0EJZr+f/Gj9fqK4w==", "MzVSe/GhHMr3eggdjkPm0GDPTLy1C2xDOyZqlgVHc2Y=", 2 },
                    { 3, "yOQWFM0EJZr+f/Gj9fqK4w==", "MzVSe/GhHMr3eggdjkPm0GDPTLy1C2xDOyZqlgVHc2Y=", 3 },
                    { 4, "yOQWFM0EJZr+f/Gj9fqK4w==", "MzVSe/GhHMr3eggdjkPm0GDPTLy1C2xDOyZqlgVHc2Y=", 4 },
                    { 5, "yOQWFM0EJZr+f/Gj9fqK4w==", "MzVSe/GhHMr3eggdjkPm0GDPTLy1C2xDOyZqlgVHc2Y=", 5 }
                });

            migrationBuilder.InsertData(
                table: "Vinculos",
                columns: new[] { "Id", "AgroindustriaId", "DataCriacao", "GranjaId", "RoleId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4258), null, 1, 1 },
                    { 3, 1, new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4267), null, 3, 3 },
                    { 2, 1, new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4264), 1, 2, 2 },
                    { 4, 1, new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4270), 1, 4, 4 },
                    { 5, 1, new DateTime(2025, 4, 1, 5, 48, 29, 181, DateTimeKind.Utc).AddTicks(4272), 1, 5, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vinculos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vinculos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vinculos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vinculos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vinculos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Agroindustrias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Agroindustrias",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
