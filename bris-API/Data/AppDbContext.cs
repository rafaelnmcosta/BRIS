using Microsoft.EntityFrameworkCore;
using bris_API.Models;
using bris_API.Services;

namespace bris_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Agroindustria> Agroindustrias { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Dose> Doses { get; set; }
        public DbSet<Granja> Granjas { get; set; }
        public DbSet<GranjaUsuarioTipo> GranjasUsuariosTipos { get; set; }
        public DbSet<Semana> Semanas { get; set; }
        public DbSet<Senha> Senhas { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relação 1-1 Usuario-Senha
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Senha)
                .WithOne(s => s.Usuario)
                .HasForeignKey<Senha>(s => s.UsuarioId);
            
            // Relação 1-N Agroindustria-Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Agroindustria)
                .WithMany(a => a.Usuarios)
                .HasForeignKey(u => u.AgroindustriaId);

            // Relação 1-N Agroindustria-Granja
            modelBuilder.Entity<Granja>()
                .HasOne(g => g.Agroindustria)
                .WithMany(a => a.Granjas)
                .HasForeignKey(g => g.AgroindustriaId);

            // Relação 1-N Granja-Animal
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Granja)
                .WithMany(g => g.Animais)
                .HasForeignKey(a => a.GranjaId);

            // Relação 1-N Usuario-Animal
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Animais)
                .HasForeignKey(a => a.UsuarioResponsavelId);

            // Relação 1-N Animal-Avaliacao
            modelBuilder.Entity<Avaliacao>()
                .HasOne(av => av.Animal)
                .WithMany(a => a.Avaliacoes)
                .HasForeignKey(av => av.AnimalId);

            // Relação 1-N Avaliacao-Semana
            modelBuilder.Entity<Semana>()
                .HasOne(s => s.Avaliacao)
                .WithMany(av => av.Semanas)
                .HasForeignKey(s => s.AvaliacaoId);

            // Relação 1-N Semana-Dose
            modelBuilder.Entity<Dose>()
                .HasOne(d => d.Semana)
                .WithMany(s => s.Doses)
                .HasForeignKey(d => d.SemanaId);

            // Relação 1-N Usuario-Dose
            modelBuilder.Entity<Dose>()
                .HasOne(d => d.Usuario)
                .WithMany(u => u.Doses)
                .HasForeignKey(d => d.UsuarioId);

            // Relação N-N Granja_Usuario_Tipo
            modelBuilder.Entity<GranjaUsuarioTipo>()
                .HasOne(gut => gut.Granja)
                .WithMany(g => g.GranjasUsuariosTipos)
                .HasForeignKey(gut => gut.GranjaId);

            modelBuilder.Entity<GranjaUsuarioTipo>()
                .HasOne(gut => gut.Usuario)
                .WithMany(u => u.GranjasUsuariosTipos)
                .HasForeignKey(gut => gut.UsuarioId);

            modelBuilder.Entity<GranjaUsuarioTipo>()
                .HasOne(gut => gut.TipoUsuario)
                .WithMany(t => t.GranjasUsuariosTipos)
                .HasForeignKey(gut => gut.TipoUsuarioId);

            // Inserção de dados iniciais para a tabela TiposUsuario
            modelBuilder.Entity<TipoUsuario>().HasData(
                new TipoUsuario { Id = 1, Tipo = "ADMIN", Descricao = "Administrador do sistema" },
                new TipoUsuario { Id = 2, Tipo = "GESTOR_GRANJA", Descricao = "Gestor de granjas" },
                new TipoUsuario { Id = 3, Tipo = "GESTOR_AGRO", Descricao = "Gestor de agroindústrias" },
                new TipoUsuario { Id = 4, Tipo = "TECNICO", Descricao = "Técnico de campo" },
                new TipoUsuario { Id = 5, Tipo = "VISUALIZADOR", Descricao = "Usuário com acesso somente leitura" },
                new TipoUsuario { Id = 98, Tipo = "PENDENTE", Descricao = "Usuário pendente de ativação" },
                new TipoUsuario { Id = 99, Tipo = "INATIVO", Descricao = "Usuário inativo" }
            );

            // Dados iniciais para Agroindustria
            modelBuilder.Entity<Agroindustria>().HasData(
                new Agroindustria { Id = 1, NomeFantasia = "Agroindustria Default", RazaoSocial = "Agroindustria Default", CNPJ = "00000000000100", Ativo = true}
            );

            // Dados iniciais para Granja
            modelBuilder.Entity<Granja>().HasData(
                new Granja { Id = 1, NomePropriedade = "Granja Teste", AgroindustriaId = 1, Endereco = "Rua teste", CNPJ = "99999999000199", Ativo = true }
            );

            // Dados iniciais para o Admin
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nome = "Admin", Email = "admin@gmail.com", CPF = "00000000000", AgroindustriaId = 1 }
            );

            var salt = PasswordService.GenerateSalt();
            var hash = PasswordService.HashPassword("123456", salt);

            // Dados da senha para o Admin
            modelBuilder.Entity<Senha>().HasData(
                new Senha { Id = 1, UsuarioId = 1, SenhaHash = hash, Salt = salt }
            );

            // Linka todos os Dados na tabela de relação
            modelBuilder.Entity<GranjaUsuarioTipo>().HasData(
                new GranjaUsuarioTipo { Id = 1, UsuarioId = 1, GranjaId = 1, TipoUsuarioId = 1 }
            );

            // Aplica as configurações
            base.OnModelCreating(modelBuilder);
        }
    }
}
