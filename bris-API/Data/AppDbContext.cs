using Microsoft.EntityFrameworkCore;
using bris_API.Models;

namespace bris_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Agroindustria> Agroindustrias { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Dose> Doses { get; set; }
        public DbSet<Granja> Granjas { get; set; }
        public DbSet<Granja_Usuario_Tipo> Granjas_Usuarios_Tipos { get; set; }
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

    // Relação 1-N Usuario-Animal (um usuário cadastra vários animais)
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

    // Relação 1-N Usuario-Dose (um usuário registra várias doses)
    modelBuilder.Entity<Dose>()
        .HasOne(d => d.Usuario)
        .WithMany(u => u.Doses)
        .HasForeignKey(d => d.UsuarioId);

    // Relação N-N Granja_Usuario_Tipo (um usuário pode ter diferentes tipos em diferentes granjas)
    modelBuilder.Entity<Granja_Usuario_Tipo>()
        .HasKey(gut => new { gut.GranjaId, gut.UsuarioId, gut.TipoUsuarioId });

    modelBuilder.Entity<Granja_Usuario_Tipo>()
        .HasOne(gut => gut.Granja)
        .WithMany(g => g.Granjas_Usuarios_Tipos)
        .HasForeignKey(gut => gut.GranjaId);

    modelBuilder.Entity<Granja_Usuario_Tipo>()
        .HasOne(gut => gut.Usuario)
        .WithMany(u => u.Granjas_Usuarios_Tipos)
        .HasForeignKey(gut => gut.UsuarioId);

    modelBuilder.Entity<Granja_Usuario_Tipo>()
        .HasOne(gut => gut.TipoUsuario)
        .WithMany(t => t.Granjas_Usuarios_Tipos)
        .HasForeignKey(gut => gut.TipoUsuarioId);
}

    }
}
