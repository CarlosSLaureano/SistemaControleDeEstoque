using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Repositorio
{
    public interface ICategoriaRepositorio
    {
        CategoriaModel ListarPorId(int id);
        List<CategoriaModel> BuscarTodos();
        CategoriaModel Adicionar(CategoriaModel categoria);
        CategoriaModel Atualizar(CategoriaModel categoria);
        bool Apagar(int id);
        
        
        
    }
}
