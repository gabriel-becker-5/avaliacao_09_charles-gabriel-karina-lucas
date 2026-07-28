using avaliacao_09_charles_gabriel_karina_lucas.Data;
using avaliacao_09_charles_gabriel_karina_lucas.Interfaces;
using avaliacao_09_charles_gabriel_karina_lucas.Models;
using avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace avaliacao_09_charles_gabriel_karina_lucas.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

        public async Task<bool> CadastrarUsuario(CadastroUsuarioViewModel novoUsuario)
        {
            Usuario? usuarioJaCadastrado = _context.Usuario.Where(u => u.Email == novoUsuario.Email).FirstOrDefault();

            if (usuarioJaCadastrado == null)
            {
                Usuario usuario = new Usuario
                {
                    Email = novoUsuario.Email,
                    Nome = novoUsuario.Nome
                };
                usuario.SenhaHash = passwordHasher.HashPassword(usuario, novoUsuario.Senha);
                await _context.Usuario.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Usuario? BuscarUsuario(LoginUsuarioViewModel usuarioLogin)
        {
            Usuario? usuarioEncontrado = _context.Usuario.Where(u => u.Email == usuarioLogin.Email).FirstOrDefault();
            return usuarioEncontrado;
        }

        public async Task<bool> HashSenhaEhValida(Usuario usuario, LoginUsuarioViewModel usuarioLogin)
        {
            PasswordVerificationResult verificaHashDaSenha = passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, usuarioLogin.Senha);

            if (verificaHashDaSenha == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}