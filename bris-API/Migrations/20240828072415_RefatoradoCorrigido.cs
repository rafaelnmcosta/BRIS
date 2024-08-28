using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace bris_API.Migrations
{
    /// <inheritdoc />
    public partial class RefatoradoCorrigido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_TiposUsuario_TipoUsuarioId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Granjas_Usuarios_Tipos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TipoUsuarioId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TipoUsuarioId",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Nome_propriedade",
                table: "Granjas",
                newName: "NomePropriedade");

            migrationBuilder.RenameColumn(
                name: "DataResgistro",
                table: "Doses",
                newName: "DataRegistro");

            migrationBuilder.RenameColumn(
                name: "DataInicioAvaliacap",
                table: "Avaliacoes",
                newName: "DataInicioAvaliacao");

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Peso",
                table: "Animais",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "GranjasUsuariosTipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    GranjaId = table.Column<int>(type: "integer", nullable: true),
                    TipoUsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GranjasUsuariosTipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GranjasUsuariosTipos_Granjas_GranjaId",
                        column: x => x.GranjaId,
                        principalTable: "Granjas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GranjasUsuariosTipos_TiposUsuario_TipoUsuarioId",
                        column: x => x.TipoUsuarioId,
                        principalTable: "TiposUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GranjasUsuariosTipos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GranjasUsuariosTipos",
                columns: new[] { "Id", "GranjaId", "TipoUsuarioId", "UsuarioId" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "xG5uQnY/KXtXQXJY3J779Q==", "WRU3XScAPAlQZlCJzof8sSImisXrbZ8PWZ5dQ1C4PZk=" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "CPF",
                value: "000.000.000-00");

            migrationBuilder.CreateIndex(
                name: "IX_GranjasUsuariosTipos_GranjaId",
                table: "GranjasUsuariosTipos",
                column: "GranjaId");

            migrationBuilder.CreateIndex(
                name: "IX_GranjasUsuariosTipos_TipoUsuarioId",
                table: "GranjasUsuariosTipos",
                column: "TipoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_GranjasUsuariosTipos_UsuarioId",
                table: "GranjasUsuariosTipos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GranjasUsuariosTipos");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "NomePropriedade",
                table: "Granjas",
                newName: "Nome_propriedade");

            migrationBuilder.RenameColumn(
                name: "DataRegistro",
                table: "Doses",
                newName: "DataResgistro");

            migrationBuilder.RenameColumn(
                name: "DataInicioAvaliacao",
                table: "Avaliacoes",
                newName: "DataInicioAvaliacap");

            migrationBuilder.AddColumn<int>(
                name: "TipoUsuarioId",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Peso",
                table: "Animais",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateTable(
                name: "Granjas_Usuarios_Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GranjaId = table.Column<int>(type: "integer", nullable: false),
                    TipoUsuarioId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Granjas_Usuarios_Tipos",
                columns: new[] { "Id", "GranjaId", "TipoUsuarioId", "UsuarioId" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.UpdateData(
                table: "Senhas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Salt", "SenhaHash" },
                values: new object[] { "TB+yey2pFxr+UXRe9EmyMg==", "44nGqQYy7DxWI0wpM7ujF2q4M0q/COs8gh4ekW+a7Jc=" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoUsuarioId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TipoUsuarioId",
                table: "Usuarios",
                column: "TipoUsuarioId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_TiposUsuario_TipoUsuarioId",
                table: "Usuarios",
                column: "TipoUsuarioId",
                principalTable: "TiposUsuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
