using System.ComponentModel.DataAnnotations;

namespace avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels
{
    public class CadastroUsuarioViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório!")]
        [Length(10, 100, ErrorMessage = "A senha informada é muito curta")]
        public string Senha { get; set; }
        
        [Required]
        [Length(3, 150, ErrorMessage = "Verifique o nome digitado.")]
        public string Nome { get; set; }
    }
}