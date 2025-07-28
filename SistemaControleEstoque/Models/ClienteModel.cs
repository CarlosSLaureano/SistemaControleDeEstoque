using SistemaControleEstoque.Enums;
using SistemaControleEstoque.Helper;
using System.ComponentModel.DataAnnotations;

namespace SistemaControleEstoque.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do cliente")]
        [StringLength(50, ErrorMessage = "O nome pode ter no máximo 50 caracteres")]
        [RegularExpression(@"^[A-ZÀ-Ý][a-zà-ÿA-ZÀ-Ý\s]*$", ErrorMessage = "O nome deve começar com letra maiúscula")]
        public string? Nome { get; set; }


        [Required(ErrorMessage = "Digite o e-mail do cliente")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Digite o telefone do cliente")]
        [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$", ErrorMessage = "O telefone informado não é válido")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Digite a data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }


        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

      
    }
}