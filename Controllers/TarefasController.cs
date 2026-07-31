using avaliacao_09_charles_gabriel_karina_lucas.Data;
using avaliacao_09_charles_gabriel_karina_lucas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace avaliacao_09_charles_gabriel_karina_lucas.Controllers
{
    [Authorize]
    public class TarefasController : Controller
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        private int ObterUsuarioId()
        {
            string? usuarioId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(usuarioId, out int id))
            {
                throw new UnauthorizedAccessException(
                    "Não foi possível identificar o usuário logado.");
            }

            return id;
        }

        // Lista somente as tarefas do usuário logado
        public async Task<IActionResult> Index()
        {
            int usuarioId = ObterUsuarioId();

            var tarefas = await _context.Tarefas
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();

            return View(tarefas);
        }

        // Exibe somente uma tarefa pertencente ao usuário logado
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int usuarioId = ObterUsuarioId();

            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UsuarioId == usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                tarefa.UsuarioId = ObterUsuarioId();

                _context.Add(tarefa);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tarefa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int usuarioId = ObterUsuarioId();

            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UsuarioId == usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return NotFound();
            }

            int usuarioId = ObterUsuarioId();

            var tarefaExistente = await _context.Tarefas
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UsuarioId == usuarioId);

            if (tarefaExistente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza somente os campos permitidos.
                    tarefaExistente.Titulo = tarefa.Titulo;
                    tarefaExistente.Descricao = tarefa.Descricao;
                    tarefaExistente.Data = tarefa.Data;
                    tarefaExistente.Concluida = tarefa.Concluida;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Id, usuarioId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(tarefa);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int usuarioId = ObterUsuarioId();

            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UsuarioId == usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int usuarioId = ObterUsuarioId();

            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UsuarioId == usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExists(int id, int usuarioId)
        {
            return _context.Tarefas.Any(t =>
                t.Id == id &&
                t.UsuarioId == usuarioId);
        }
    }
}