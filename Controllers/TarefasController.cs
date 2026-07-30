using avaliacao_09_charles_gabriel_karina_lucas.Models;
using Microsoft.AspNetCore.Mvc;

namespace avaliacao_09_charles_gabriel_karina_lucas.Controllers
{
    public class TarefasController : Controller
    {
        public IActionResult Index() =>
            View("Index", new List<Tarefa> {
            new Tarefa { Id = 1, Titulo = "Teste", Descricao = "Desc", Data = DateTime.Now, Concluida = false }
            });

        public IActionResult Create() => View("Create", new Tarefa());
        public IActionResult Details() => View("Details", new Tarefa { Id = 1, Titulo = "Teste", Descricao = "Desc" });
        public IActionResult Edit() => View("Edit", new Tarefa { Id = 1, Titulo = "Teste" });
        public IActionResult Delete() => View("Delete", new Tarefa { Id = 1, Titulo = "Teste" });
    }
}