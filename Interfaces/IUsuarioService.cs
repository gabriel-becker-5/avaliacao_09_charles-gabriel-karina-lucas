using avaliacao_09_charles_gabriel_karina_lucas.Models;
using avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels;

namespace avaliacao_09_charles_gabriel_karina_lucas.Interfaces
{
    public interface IUsuarioService
    {
        public Task<bool> CadastrarUsuario(CadastroUsuarioViewModel novoUsuario);

        public Usuario? BuscarUsuario(LoginUsuarioViewModel usuarioLogin);

        public Task<bool> HashSenhaEhValida(Usuario usuario, LoginUsuarioViewModel usuarioLogin);
    }
}