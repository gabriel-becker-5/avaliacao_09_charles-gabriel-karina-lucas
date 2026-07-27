using avaliacao_09_charles_gabriel_karina_lucas.Interfaces;
using avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace avaliacao_09_charles_gabriel_karina_lucas.Controllers
{
    [Route("usuario")]
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IdUsuario"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }

        [AllowAnonymous]
        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar(CadastroUsuarioViewModel novoUsuario)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!await _usuarioService.CadastrarUsuario(novoUsuario))
            {
                ViewBag.DuplicateUser = "Usuário já cadastrado!";
                return View();
            }

            return RedirectToAction("Index");

        }

        [AllowAnonymous]
        [HttpGet("logar")]
        public IActionResult Logar()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("logar")]
        public async Task<IActionResult> Logar(CadastroUsuarioViewModel usuarioLogin)
        {
            var usuarioEncontrado = _usuarioService.BuscarUsuario(usuarioLogin);

            if (usuarioEncontrado == null)
            {
                ViewBag.CredentialError = "Usuário ou Senha incorretos";
                return View();
            }

            bool validadeSenha = await _usuarioService.HashSenhaEhValida(usuarioEncontrado, usuarioLogin);

            if (!validadeSenha)
            {
                ViewBag.CredentialError = "Usuário ou Senha incorretos";
                return View();
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioEncontrado.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuarioEncontrado.Nome)
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Logar");
        }
    }
}