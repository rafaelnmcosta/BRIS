using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Agroindustrias",
                columns: new[] { "Id", "Ativo", "CNPJ", "DataCadastro", "Email", "Endereco", "NomeFantasia", "RazaoSocial", "Telefone" },
                values: new object[,]
                {
                    { 1, true, "00000000000101", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4504), "agro_teste@gomail.com", "Rua teste 1", "Agroindustria Teste", "Agroindustria Default", "62999999911" },
                    { 2, true, "11111111000102", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4512), "agro_nova@gomail.com", "Rua teste 2", "Agroindustria Nova", "Agroindustria Nova", "62999999911" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CPF", "DataCadastro", "Email", "Nome", "Telefone", "UltimoLogin" },
                values: new object[,]
                {
                    { 1, "11111111111", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4716), "admin@gomail.com", "Admin", "62999999901", null },
                    { 2, "22222222222", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4721), "gestor_agro@gomail.com", "Gestor Agro", "62999999902", null },
                    { 3, "33333333333", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4724), "gestor_granja@gomail.com", "Gestor Granja", "62999999903", null },
                    { 4, "44444444444", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4728), "tecnico@gomail.com", "Tecnico", "62999999904", null },
                    { 5, "55555555555", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4731), "visualizador@gomail.com", "Visualizador", "62999999905", null }
                });

            migrationBuilder.InsertData(
                table: "Granjas",
                columns: new[] { "Id", "AgroindustriaId", "Ativo", "CNPJ", "DataCadastro", "Email", "NomePropriedade", "Telefone" },
                values: new object[,]
                {
                    { 1, 1, true, "99999999000111", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4639), "granja_teste1@gomail.com", "Granja Teste 1", "62999999921" },
                    { 2, 1, true, "99999999000112", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4644), "granja_teste2@gomail.com", "Granja Teste 2", "62999999922" },
                    { 3, 2, true, "88888888000121", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4647), "granja_nova1@gomail.com", "Granja Nova 1", "62999999931" },
                    { 4, 2, true, "88888888000122", new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(4650), "granja_nova2@gomail.com", "Granja Nova 2", "62999999932" }
                });

            migrationBuilder.InsertData(
                table: "Senhas",
                columns: new[] { "Id", "Salt", "SenhaHash", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "k9OPqqPqqNebyi+17uH3Pg==", "InC+6axnxVbVlVk1BdF9Mn1vGlfhsh0gzB6Mi35vRHc=", 1 },
                    { 2, "grBDbVdlfIi1dcxHmbQkPA==", "f+DmjXMUCnBUtLPJedpWLMlwYOLASKcPuDoLU7vOZK4=", 2 },
                    { 3, "I+lhpCPV7AXgA+M0CLPSpw==", "YSdAQq/U1g18QvS2pJdqXnFdwgIo5eC3uDqmeAR/RDE=", 3 },
                    { 4, "d56K7xBokBNFC9TBZ9FKbQ==", "NhOXWS9FzbSdFXrO6bqMc5tPAzbrHtJE6q/ZZOzWhSk=", 4 },
                    { 5, "ObLMi+gdZkSRoTZNl79WYQ==", "fwojFUmlJv95R5K58x0+btX7LBjnkjbdCu5aEzsr3O4=", 5 }
                });

            migrationBuilder.InsertData(
                table: "Vinculos",
                columns: new[] { "Id", "AgroindustriaId", "DataCriacao", "GranjaId", "RoleId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(6147), null, 1, 1 },
                    { 2, 1, new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(6154), null, 2, 2 },
                    { 3, 1, new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(6157), 1, 3, 3 },
                    { 4, 1, new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(6159), 1, 4, 4 },
                    { 5, 1, new DateTime(2025, 5, 13, 16, 11, 57, 116, DateTimeKind.Utc).AddTicks(6162), 1, 5, 5 }
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
