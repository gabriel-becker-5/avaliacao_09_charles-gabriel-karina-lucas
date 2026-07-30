using avaliacao_09_charles_gabriel_karina_lucas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace avaliacao_09_charles_gabriel_karina_lucas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("index", "usuario");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}