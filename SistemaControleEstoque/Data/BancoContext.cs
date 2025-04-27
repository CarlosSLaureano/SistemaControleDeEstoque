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

       
    }
}

