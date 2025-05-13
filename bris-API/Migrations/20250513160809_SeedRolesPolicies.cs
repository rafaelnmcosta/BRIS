using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Policy",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Policy para VisualizaTotal", "VisualizaTotal" },
                    { 2, "Policy para VisualizaAgroindustria", "VisualizaAgroindustria" },
                    { 3, "Policy para VisualizaUsuarios", "VisualizaUsuarios" },
                    { 4, "Policy para VisualizaAnimais", "VisualizaAnimais" },
                    { 5, "Policy para GerenciaTotal", "GerenciaTotal" },
                    { 6, "Policy para GerenciaAgroindustria", "GerenciaAgroindustria" },
                    { 7, "Policy para GerenciaUsuarios", "GerenciaUsuarios" },
                    { 8, "Policy para GerenciaAnimais", "GerenciaAnimais" },
                    { 9, "Policy para TodosUsuarios", "TodosUsuarios" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Administrador do sistema", "ADMIN" },
                    { 2, "Gestor de agroindústrias", "GESTOR_AGRO" },
                    { 3, "Gestor de granjas", "GESTOR_GRANJA" },
                    { 4, "Técnico da granja", "TECNICO" },
                    { 5, "Usuário com acesso somente de visualização ao sistema", "VISUALIZADOR" },
                    { 98, "Usuário pendente de ativação", "PENDENTE" },
                    { 99, "Usuário inativo", "INATIVO" }
                });

            migrationBuilder.InsertData(
                table: "PolicyRoles",
                columns: new[] { "Id", "PolicyId", "RoleId" },
                values: new object[,]
                {
                    { 11, 1, 1 },
                    { 21, 2, 1 },
                    { 22, 2, 2 },
                    { 25, 2, 5 },
                    { 31, 3, 1 },
                    { 32, 3, 2 },
                    { 33, 3, 3 },
                    { 35, 3, 5 },
                    { 41, 4, 1 },
                    { 42, 4, 2 },
                    { 43, 4, 3 },
                    { 44, 4, 4 },
                    { 45, 4, 5 },
                    { 51, 5, 1 },
                    { 61, 6, 1 },
                    { 62, 6, 2 },
                    { 71, 7, 1 },
                    { 72, 7, 2 },
                    { 73, 7, 3 },
                    { 81, 8, 1 },
                    { 83, 8, 3 },
                    { 84, 8, 4 },
                    { 91, 9, 1 },
                    { 92, 9, 2 },
                    { 93, 9, 3 },
                    { 94, 9, 4 },
                    { 95, 9, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "PolicyRoles",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
