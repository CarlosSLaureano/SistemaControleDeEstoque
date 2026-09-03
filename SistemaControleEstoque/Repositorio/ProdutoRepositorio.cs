using Microsoft.EntityFrameworkCore;
using SistemaControleEstoque.Data;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _bancoContext;

        public ProdutoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ProdutoModel ListarPorId(int id)
        {
            return _bancoContext.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<ProdutoModel> BuscarTodos()
        {
            return _bancoContext.Produtos
                .Include(p => p.Categoria)
                .ToList();
        }

        public ProdutoModel Adicionar(ProdutoModel produto)
        {
            produto.DataCadastro = DateTime.Now;

            // Calcula o total antes de salvar
            if (produto.Quantidade.HasValue && produto.Preco.HasValue)
                produto.Total = produto.Quantidade.Value * produto.Preco.Value;
            else
                produto.Total = 0;

            _bancoContext.Produtos.Add(produto);
            _bancoContext.SaveChanges();
            return produto;
        }

        public ProdutoModel Atualizar(ProdutoModel produto)
        {
            ProdutoModel produtoDB = ListarPorId(produto.Id);

            if (produtoDB == null)
                throw new Exception("Houve um erro na atualização do produto!");

            // Atualiza os campos
            produtoDB.Nome = produto.Nome;
            produtoDB.Descricao = produto.Descricao;
            produtoDB.Preco = produto.Preco;
            produtoDB.Quantidade = produto.Quantidade;
            produtoDB.CategoriaId = produto.CategoriaId;
            produtoDB.DataAtualizacao = DateTime.Now;

            // Atualiza o total com base nos valores
            if (produto.Quantidade.HasValue && produto.Preco.HasValue)
                produtoDB.Total = produto.Quantidade.Value * produto.Preco.Value;
            else
                produtoDB.Total = 0;

            _bancoContext.Produtos.Update(produtoDB);
            _bancoContext.SaveChanges();

            return produtoDB;
        }

        public bool Apagar(int id)
        {
            ProdutoModel produtoDB = ListarPorId(id);

            if (produtoDB == null)
                throw new Exception("Houve um erro na deleção do produto");

            _bancoContext.Produtos.Remove(produtoDB);
            _bancoContext.SaveChanges();
            return true;
        }
    }
}

