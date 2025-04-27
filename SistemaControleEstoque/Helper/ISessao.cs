using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Helper
{
    public interface ISessao
    {
        void CriarSessaoDoUsuario (UsuarioModel usuario);
        void RemoverSessaoUsuario();
        UsuarioModel BuscarSessaoDoUsuario();

    }
}
