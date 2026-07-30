using Microsoft.AspNetCore.Mvc;

namespace avaliacao_09_charles_gabriel_karina_lucas.Controllers
{
    public class ContaController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }
    }
}