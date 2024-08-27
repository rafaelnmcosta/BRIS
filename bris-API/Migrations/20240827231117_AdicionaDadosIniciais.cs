using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDadosIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Agroindustrias",
                columns: new[] { "Id", "CNPJAgroindustria", "NomeFantasia", "RazaoSocial" },
                values: new object[] { 1, "00.000.000/0001-00", "Agroindustria Teste", "Razao Agroindustria" });

            migrationBuilder.InsertData(
                table: "TiposUsuario",
                columns: new[] { "Id", "Descricao", "Tipo" },
                values: new object[,]
                {
                    { 1, "Administrador do sistema", "ADMIN" },
                    { 2, "Gestor de granjas", "GESTOR GRANJA" },
                    { 3, "Gestor de agroindústrias", "GESTOR AGRO" },
                    { 4, "Técnico de campo", "TECNICO" },
                    { 5, "Usuário com acesso somente leitura", "VISUALIZADOR" },
                    { 98, "Usuário pendente de ativação", "PENDENTE" },
                    { 99, "Usuário inativo", "INATIVO" }
                });

            migrationBuilder.InsertData(
                table: "Granjas",
                columns: new[] { "Id", "AgroindustriaId", "CNPJ", "Endereco", "Nome_propriedade" },
                values: new object[] { 1, 1, "99.999.999/0001-99", "Rua teste", "Granja Teste" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nome", "TipoUsuarioId" },
                values: new object[] { 1, "admin@gmail.com", "Admin", 1 });

            migrationBuilder.InsertData(
                table: "Granjas_Usuarios_Tipos",
                columns: new[] { "Id", "GranjaId", "TipoUsuarioId", "UsuarioId" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Senhas",
                columns: new[] { "Id", "Salt", "SenhaHash", "UsuarioId" },
                values: new object[] { 1, "TB+yey2pFxr+UXRe9EmyMg==", "44nGqQYy7DxWI0wpM7ujF2q4M0q/COs8gh4ekW+a7Jc=", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Granjas_Usuarios_Tipos",
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
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agroindustrias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
