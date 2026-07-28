using System.ComponentModel.DataAnnotations;

namespace avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels
{
    public class LoginUsuarioViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório!")]
        public string Senha { get; set; }
    }
}