using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class ModeloInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agroindustrias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeFantasia = table.Column<string>(type: "text", nullable: true),
                    RazaoSocial = table.Column<string>(type: "text", nullable: true),
                    CNPJ = table.Column<string>(type: "text", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agroindustrias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CPF = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Granjas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomePropriedade = table.Column<string>(type: "text", nullable: true),
                    AgroindustriaId = table.Column<int>(type: "integer", nullable: false),
                    Endereco = table.Column<string>(type: "text", nullable: true),
                    CNPJ = table.Column<string>(type: "text", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Granjas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Granjas_Agroindustrias_AgroindustriaId",
                        column: x => x.AgroindustriaId,
                        principalTable: "Agroindustrias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PolicyRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    PolicyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyRoles_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PolicyRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Senhas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senhas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Senhas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Linhagem = table.Column<string>(type: "text", nullable: true),
                    Idade = table.Column<int>(type: "integer", nullable: false),
                    Peso = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true),
                    GranjaId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioResponsavelId = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animais_Granjas_GranjaId",
                        column: x => x.GranjaId,
                        principalTable: "Granjas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Animais_Usuarios_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vinculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    GranjaId = table.Column<int>(type: "integer", nullable: true),
                    AgroindustriaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vinculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vinculos_Agroindustrias_AgroindustriaId",
                        column: x => x.AgroindustriaId,
                        principalTable: "Agroindustrias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vinculos_Granjas_GranjaId",
                        column: x => x.GranjaId,
                        principalTable: "Granjas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vinculos_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vinculos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    DataInicioAvaliacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusAvaliacao = table.Column<int>(type: "integer", nullable: false),
                    ResultadoFinal = table.Column<bool>(type: "boolean", nullable: true),
                    ProximaDoseSemana = table.Column<int>(type: "integer", nullable: false),
                    ProximaDoseOrdem = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Animais_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Semanas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NroSemana = table.Column<int>(type: "integer", nullable: false),
                    Resultado = table.Column<int>(type: "integer", nullable: false),
                    AvaliacaoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semanas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Semanas_Avaliacoes_AvaliacaoId",
                        column: x => x.AvaliacaoId,
                        principalTable: "Avaliacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SemanaId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorRegistrado = table.Column<float>(type: "real", nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    PodePreencher = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doses_Semanas_SemanaId",
                        column: x => x.SemanaId,
                        principalTable: "Semanas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doses_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agroindustrias",
                columns: new[] { "Id", "Ativo", "CNPJ", "NomeFantasia", "RazaoSocial" },
                values: new object[] { 1, true, "00000000000100", "Agroindustria Default", "Agroindustria Default" });

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
                    { 2, "Gestor de granjas", "GESTOR_GRANJA" },
                    { 3, "Gestor de agroindústrias", "GESTOR_AGRO" },
                    { 4, "Técnico da granja", "TECNICO" },
                    { 5, "Usuário com acesso somente de visualização ao sistema", "VISUALIZADOR" },
                    { 98, "Usuário pendente de ativação", "PENDENTE" },
                    { 99, "Usuário inativo", "INATIVO" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CPF", "Email", "Nome" },
                values: new object[] { 1, "00000000000", "admin@gmail.com", "Admin" });

            migrationBuilder.InsertData(
                table: "Granjas",
                columns: new[] { "Id", "AgroindustriaId", "Ativo", "CNPJ", "Endereco", "NomePropriedade" },
                values: new object[] { 1, 1, true, "99999999000199", "Rua teste", "Granja Teste" });

            migrationBuilder.InsertData(
                table: "PolicyRoles",
                columns: new[] { "Id", "PolicyId", "RoleId" },
                values: new object[,]
                {
                    { 11, 1, 1 },
                    { 21, 2, 1 },
                    { 23, 2, 3 },
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
                    { 63, 6, 3 },
                    { 71, 7, 1 },
                    { 72, 7, 2 },
                    { 73, 7, 3 },
                    { 81, 8, 1 },
                    { 82, 8, 2 },
                    { 84, 8, 4 },
                    { 91, 9, 1 },
                    { 92, 9, 2 },
                    { 93, 9, 3 },
                    { 94, 9, 4 },
                    { 95, 9, 5 }
                });

            migrationBuilder.InsertData(
                table: "Senhas",
                columns: new[] { "Id", "Salt", "SenhaHash", "UsuarioId" },
                values: new object[] { 1, "s91hRJFr1WMKFEi+autXuA==", "m0RFcbkJGFZbQKgnK9ca8Qzt2FPSJeRhOL0lkk9WE/k=", 1 });

            migrationBuilder.InsertData(
                table: "Vinculos",
                columns: new[] { "Id", "AgroindustriaId", "GranjaId", "RoleId", "UsuarioId" },
                values: new object[] { 1, null, null, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Animais_GranjaId",
                table: "Animais",
                column: "GranjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Animais_UsuarioResponsavelId",
                table: "Animais",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_AnimalId",
                table: "Avaliacoes",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Doses_SemanaId",
                table: "Doses",
                column: "SemanaId");

            migrationBuilder.CreateIndex(
                name: "IX_Doses_UsuarioId",
                table: "Doses",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Granjas_AgroindustriaId",
                table: "Granjas",
                column: "AgroindustriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoles_PolicyId",
                table: "PolicyRoles",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoles_RoleId",
                table: "PolicyRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Semanas_AvaliacaoId",
                table: "Semanas",
                column: "AvaliacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Senhas_UsuarioId",
                table: "Senhas",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_AgroindustriaId",
                table: "Vinculos",
                column: "AgroindustriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_GranjaId",
                table: "Vinculos",
                column: "GranjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_RoleId",
                table: "Vinculos",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_UsuarioId",
                table: "Vinculos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doses");

            migrationBuilder.DropTable(
                name: "PolicyRoles");

            migrationBuilder.DropTable(
                name: "Senhas");

            migrationBuilder.DropTable(
                name: "Vinculos");

            migrationBuilder.DropTable(
                name: "Semanas");

            migrationBuilder.DropTable(
                name: "Policy");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "Animais");

            migrationBuilder.DropTable(
                name: "Granjas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Agroindustrias");
        }
    }
}
