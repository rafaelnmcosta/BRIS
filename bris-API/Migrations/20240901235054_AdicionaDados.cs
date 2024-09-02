using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Agroindustrias",
                columns: new[] { "Id", "Ativo", "CNPJ", "NomeFantasia", "RazaoSocial" },
                values: new object[] { 1, true, "00000000000100", "Agroindustria Default", "Agroindustria Default" });

            migrationBuilder.InsertData(
                table: "TiposUsuario",
                columns: new[] { "Id", "Descricao", "Tipo" },
                values: new object[,]
                {
                    { 1, "Administrador do sistema", "ADMIN" },
                    { 2, "Gestor de granjas", "GESTOR_GRANJA" },
                    { 3, "Gestor de agroindústrias", "GESTOR_AGRO" },
                    { 4, "Técnico de campo", "TECNICO" },
                    { 5, "Usuário com acesso somente leitura", "VISUALIZADOR" },
                    { 98, "Usuário pendente de ativação", "PENDENTE" },
                    { 99, "Usuário inativo", "INATIVO" }
                });

            migrationBuilder.InsertData(
                table: "Granjas",
                columns: new[] { "Id", "AgroindustriaId", "Ativo", "CNPJ", "Endereco", "NomePropriedade" },
                values: new object[] { 1, 1, true, "99999999000199", "Rua teste", "Granja Teste" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "AgroindustriaId", "CPF", "Email", "Nome" },
                values: new object[] { 1, 1, "00000000000", "admin@gmail.com", "Admin" });

            migrationBuilder.InsertData(
                table: "GranjasUsuariosTipos",
                columns: new[] { "Id", "GranjaId", "TipoUsuarioId", "UsuarioId" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Senhas",
                columns: new[] { "Id", "Salt", "SenhaHash", "UsuarioId" },
                values: new object[] { 1, "4sNW+s6WkEuuKlolORFIjw==", "sJ+5RoVZtxR1SBQtdURuHRKtpEgmwMTT9TG1JTnz8uc=", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GranjasUsuariosTipos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Granjas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agroindustrias",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
