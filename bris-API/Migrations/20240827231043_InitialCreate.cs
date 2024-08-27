using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    CNPJAgroindustria = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agroindustrias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Granjas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome_propriedade = table.Column<string>(type: "text", nullable: true),
                    AgroindustriaId = table.Column<int>(type: "integer", nullable: false),
                    Endereco = table.Column<string>(type: "text", nullable: true),
                    CNPJ = table.Column<string>(type: "text", nullable: true)
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
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    TipoUsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_TiposUsuario_TipoUsuarioId",
                        column: x => x.TipoUsuarioId,
                        principalTable: "TiposUsuario",
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
                    Peso = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    GranjaId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioResponsavelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animais_Granjas_GranjaId",
                        column: x => x.GranjaId,
                        principalTable: "Granjas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animais_Usuarios_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Granjas_Usuarios_Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    GranjaId = table.Column<int>(type: "integer", nullable: false),
                    TipoUsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Granjas_Usuarios_Tipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Granjas_Usuarios_Tipos_Granjas_GranjaId",
                        column: x => x.GranjaId,
                        principalTable: "Granjas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Granjas_Usuarios_Tipos_TiposUsuario_TipoUsuarioId",
                        column: x => x.TipoUsuarioId,
                        principalTable: "TiposUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Granjas_Usuarios_Tipos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
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
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    DataInicioAvaliacap = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusAvaliacao = table.Column<int>(type: "integer", nullable: false),
                    ResultadoFinal = table.Column<bool>(type: "boolean", nullable: false)
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
                    DataResgistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValorRegistrado = table.Column<float>(type: "real", nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Granjas_Usuarios_Tipos_GranjaId",
                table: "Granjas_Usuarios_Tipos",
                column: "GranjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Granjas_Usuarios_Tipos_TipoUsuarioId",
                table: "Granjas_Usuarios_Tipos",
                column: "TipoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Granjas_Usuarios_Tipos_UsuarioId",
                table: "Granjas_Usuarios_Tipos",
                column: "UsuarioId");

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
                name: "IX_Usuarios_TipoUsuarioId",
                table: "Usuarios",
                column: "TipoUsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doses");

            migrationBuilder.DropTable(
                name: "Granjas_Usuarios_Tipos");

            migrationBuilder.DropTable(
                name: "Senhas");

            migrationBuilder.DropTable(
                name: "Semanas");

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

            migrationBuilder.DropTable(
                name: "TiposUsuario");
        }
    }
}
