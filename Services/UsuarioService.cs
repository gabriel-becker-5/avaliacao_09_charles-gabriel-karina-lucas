using avaliacao_09_charles_gabriel_karina_lucas.Data;
using avaliacao_09_charles_gabriel_karina_lucas.Interfaces;
using avaliacao_09_charles_gabriel_karina_lucas.Models;
using avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
            Usuario usuarioJaCadastrado = _context.Usuario.Where(u => u.Email == novoUsuario.Email).FirstOrDefault();

            if (usuarioJaCadastrado == null)
            {
                Usuario usuario = new Usuario();
                usuario.Email = novoUsuario.Email;
                usuario.SenhaHash = passwordHasher.HashPassword(usuario, novoUsuario.Senha);
                usuario.Nome = novoUsuario.Nome;
                await _context.Usuario.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Usuario? BuscarUsuario(CadastroUsuarioViewModel usuarioLogin)
        {
            Usuario usuarioEncontrado = _context.Usuario.Where(u => u.Email == usuarioLogin.Email).FirstOrDefault();
            return usuarioEncontrado;
        }

        public async Task<bool> HashSenhaEhValida(Usuario usuario, CadastroUsuarioViewModel usuarioLogin)
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