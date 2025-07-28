using System.ComponentModel.DataAnnotations;

namespace SistemaControleEstoque.Models
{
    public class AlterarSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite a senha atual do usuário")]
        public required string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Digite a nova senha do usuário")]
        public required string NovaSenha { get; set; }

        [Required(ErrorMessage = "Confirme a nova senha do usuário")]
        [Compare("NovaSenha", ErrorMessage = "Senha não confere com a nova senha")]
        public required string ConfirmarNovaSenha { get; set; }
    }
}
