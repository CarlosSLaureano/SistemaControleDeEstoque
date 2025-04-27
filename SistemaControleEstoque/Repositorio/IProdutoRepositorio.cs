using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Repositorio
{
     public interface IProdutoRepositorio
     {
            ProdutoModel ListarPorId(int id);
            List<ProdutoModel> BuscarTodos();
            ProdutoModel Adicionar(ProdutoModel produto);
            ProdutoModel Atualizar(ProdutoModel produto);
            bool Apagar(int id);

     }
    
}
