using SistemaControleEstoque.Data;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly  BancoContext _bancoContext;
        public CategoriaRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public CategoriaModel ListarPorId(int id)
        {
            return _bancoContext.Categorias.FirstOrDefault(x => x.Id == id);

        }
        public List<CategoriaModel> BuscarTodos()
        {
            return _bancoContext.Categorias.ToList();
            
        }
        public CategoriaModel Adicionar(CategoriaModel categoria)
        {
            categoria.DataCadastro = DateTime.Now;
            _bancoContext.Categorias.Add(categoria);
            _bancoContext.SaveChanges();
            return categoria;
        }

        public CategoriaModel Atualizar(CategoriaModel categoria)
        {
            CategoriaModel categoriaDB = ListarPorId(categoria.Id);

            if (categoriaDB == null) throw new System.Exception("Houve um erro na atualização da categoria");
            categoriaDB.Nome = categoria.Nome;
            categoriaDB.DataAtualizacao = DateTime.Now;

            _bancoContext.Categorias.Update(categoriaDB);
            _bancoContext.SaveChanges();
            return categoriaDB;

        }

        public bool Apagar(int id)
        {
            CategoriaModel categoriaDB = ListarPorId(id);

            if (categoriaDB == null) throw new System.Exception("Houve um erro na deleção da categoria");

            _bancoContext.Categorias.Remove(categoriaDB);
            _bancoContext.SaveChanges();
            return true;

        }
    }
}
