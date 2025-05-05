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
                    { 1, true, "00000000000101", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(6813), "Agroindustria Default", "Agroindustria Default" },
                    { 2, true, "11111111000102", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(6819), "Agroindustria Nova", "Agroindustria Nova" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CPF", "DataCadastro", "Email", "Nome", "Telefone", "UltimoLogin" },
                values: new object[,]
                {
                    { 1, "11111111111", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8479), "admin@gmail.com", "Admin", null, null },
                    { 2, "22222222222", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8485), "gestor_agro@gmail.com", "Gestor Agro", null, null },
                    { 3, "33333333333", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8488), "gestor_granja@gmail.com", "Gestor Granja", null, null },
                    { 4, "44444444444", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8491), "tecnico@gmail.com", "Tecnico", null, null },
                    { 5, "55555555555", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8494), "visualizador@gmail.com", "Visualizador", null, null }
                });

            migrationBuilder.InsertData(
                table: "Granjas",
                columns: new[] { "Id", "AgroindustriaId", "Ativo", "CNPJ", "DataCadastro", "Endereco", "NomePropriedade" },
                values: new object[,]
                {
                    { 1, 1, true, "99999999000111", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(6951), "Rua teste 1", "Granja Teste 1" },
                    { 2, 1, true, "99999999000112", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(6962), "Rua teste 2", "Granja Teste 2" },
                    { 3, 2, true, "88888888000121", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(6966), "Rua nova 1", "Granja Nova 1" },
                    { 4, 2, true, "88888888000122", new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(6968), "Rua nova 2", "Granja Nova 2" }
                });

            migrationBuilder.InsertData(
                table: "Senhas",
                columns: new[] { "Id", "Salt", "SenhaHash", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "mKR6COKLqCcjQurdit5B4A==", "z6ys+XBlveRcAia/roCULcK4767pYr6mzh1/VS7VQ90=", 1 },
                    { 2, "mKR6COKLqCcjQurdit5B4A==", "z6ys+XBlveRcAia/roCULcK4767pYr6mzh1/VS7VQ90=", 2 },
                    { 3, "mKR6COKLqCcjQurdit5B4A==", "z6ys+XBlveRcAia/roCULcK4767pYr6mzh1/VS7VQ90=", 3 },
                    { 4, "mKR6COKLqCcjQurdit5B4A==", "z6ys+XBlveRcAia/roCULcK4767pYr6mzh1/VS7VQ90=", 4 },
                    { 5, "mKR6COKLqCcjQurdit5B4A==", "z6ys+XBlveRcAia/roCULcK4767pYr6mzh1/VS7VQ90=", 5 }
                });

            migrationBuilder.InsertData(
                table: "Vinculos",
                columns: new[] { "Id", "AgroindustriaId", "DataCriacao", "GranjaId", "RoleId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8807), null, 1, 1 },
                    { 2, 1, new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8812), null, 2, 2 },
                    { 3, 1, new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8816), 1, 3, 3 },
                    { 4, 1, new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8819), 1, 4, 4 },
                    { 5, 1, new DateTime(2025, 5, 5, 22, 28, 13, 634, DateTimeKind.Utc).AddTicks(8822), 1, 5, 5 }
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
