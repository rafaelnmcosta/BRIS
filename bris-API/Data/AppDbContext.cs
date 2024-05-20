using Microsoft.EntityFrameworkCore;
using bris_API.Models;

namespace bris_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Porco> Porcos { get; set; }
        public DbSet<Amostra> Amostras { get; set; }
        public DbSet<ResultadoFinal> ResultadosFinais { get; set; }
        public DbSet<Semana> Semanas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<Senha> Senhas { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<UsuarioPermissao> UsuarioPermissao { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relação N-1 amostra-porco
            modelBuilder.Entity<Amostra>()
                .HasOne(a => a.Porco)
                .WithMany(p => p.Amostras)
                .HasForeignKey(a => a.PorcoId);

            // Relação N-1 amostra-semana
            modelBuilder.Entity<Amostra>()
                .HasOne(a => a.Semana)
                .WithMany(s => s.Amostras)
                .HasForeignKey(a => a.SemanaId);

            // Relação 1-1 resultado-porco
            modelBuilder.Entity<ResultadoFinal>()
                .HasOne(r => r.Porco)
                .WithOne(p => p.ResultadoFinal)
                .HasForeignKey<ResultadoFinal>(r => r.PorcoId);
            
            // Relação N-N usuario-permissao (tabela UsuarioPermissao)
            modelBuilder.Entity<UsuarioPermissao>()
                .HasKey(up => new { up.UsuarioId, up.PermissaoId });

            modelBuilder.Entity<UsuarioPermissao>()
                .HasOne(up => up.Usuario)
                .WithMany(u => u.UsuarioPermissoes)
                .HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<UsuarioPermissao>()
                .HasOne(up => up.Permissao)
                .WithMany(p => p.UsuarioPermissoes)
                .HasForeignKey(up => up.PermissaoId);
            
            // Relação 1-1 usuario-senha
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Senha)
                .WithOne(s => s.Usuario)
                .HasForeignKey<Senha>(s => s.UsuarioId);
            
            // Relação N-1 usuario-tipoUsuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.TipoUsuario)
                .WithMany(tu => tu.Usuarios)
                .HasForeignKey(u => u.TipoUsuarioId);
            
        }
    }
}
