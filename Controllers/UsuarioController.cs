using avaliacao_09_charles_gabriel_karina_lucas.Interfaces;
using avaliacao_09_charles_gabriel_karina_lucas.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace avaliacao_09_charles_gabriel_karina_lucas.Controllers
{
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
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Username"] = User.FindFirstValue(ClaimTypes.Name);
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Cadastrar()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Cadastrar(CadastroUsuarioViewModel novoUsuario)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!await _usuarioService.CadastrarUsuario(novoUsuario))
            {
                ViewData["DuplicateUser"] = "Usuário já cadastrado!";
                return View();
            }

            TempData["Success"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Logar");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Logar()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logar(LoginUsuarioViewModel usuarioLogin)
        {
            var usuarioEncontrado = _usuarioService.BuscarUsuario(usuarioLogin);

            if (usuarioEncontrado == null)
            {
                ViewData["CredentialError"] = "Usuário ou Senha incorreto.";
                return View();
            }

            if (!await _usuarioService.HashSenhaEhValida(usuarioEncontrado, usuarioLogin))
            {
                ViewData["CredentialError"] = "Usuário ou Senha incorreto.";
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Logar");
        }
    }
}