using SistemaControleEstoque.Data;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly BancoContext _bancoContext;

        public ClienteRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ClienteModel ListarPorId(int id)
        {
            return _bancoContext.Clientes.FirstOrDefault(x => x.Id == id);
        }

        public List<ClienteModel> BuscarTodos()
        {
            return _bancoContext.Clientes.ToList();
        }

        public ClienteModel Adicionar(ClienteModel cliente)
        {
            cliente.DataCadastro = DateTime.Now;

            _bancoContext.Clientes.Add(cliente);
            _bancoContext.SaveChanges();
            return cliente;
        }

        public ClienteModel Atualizar(ClienteModel cliente)
        {
            ClienteModel clienteDB = ListarPorId(cliente.Id);

            if (clienteDB == null)
                throw new System.Exception("Houve um erro na atualização do cliente!");

            clienteDB.Nome = cliente.Nome;
            clienteDB.Telefone = cliente.Telefone;
            clienteDB.DataNascimento = cliente.DataNascimento;
            clienteDB.Email = cliente.Email;
            clienteDB.DataAtualizacao = DateTime.Now;

            _bancoContext.Clientes.Update(clienteDB);
            _bancoContext.SaveChanges();
            return clienteDB;
        }

        public bool Apagar(int id)
        {
            ClienteModel clienteDB = ListarPorId(id);

            if (clienteDB == null)
                throw new System.Exception("Houve um erro na deleção do cliente");

            _bancoContext.Clientes.Remove(clienteDB);
            _bancoContext.SaveChanges();
            return true;
        }
    }
}
