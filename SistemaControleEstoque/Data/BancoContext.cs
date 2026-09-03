using Microsoft.EntityFrameworkCore;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar precisão decimal para ProdutoModel
            modelBuilder.Entity<ProdutoModel>(entity =>
            {
                entity.Property(p => p.Preco).HasPrecision(18, 2);
                entity.Property(p => p.Total).HasPrecision(18, 2);

                // Relacionamento: Produto -> Categoria (opcional)
                // Ao apagar uma categoria, o CategoriaId do produto vira null (não apaga o produto)
                entity.HasOne(p => p.Categoria)
                      .WithMany()
                      .HasForeignKey(p => p.CategoriaId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}


