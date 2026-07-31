using avaliacao_09_charles_gabriel_karina_lucas.Interfaces;
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
        private readonly ITarefaRepositorio _tarefaRepositorio;

        public TarefasController(ITarefaRepositorio tarefaRepositorio)
        {
            _tarefaRepositorio = tarefaRepositorio;
        }

        private int ObterUsuarioId()
        {
            string? usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(usuarioId, out int id))
            {
                throw new UnauthorizedAccessException("Não foi possível identificar o usuário logado.");
            }

            return id;
        }

      
        public async Task<IActionResult> Index(string? statusFiltro)
        {
            int usuarioId = ObterUsuarioId();
            var tarefas = await _tarefaRepositorio.ObterTodasPorUsuarioAsync(usuarioId, statusFiltro);

            ViewBag.StatusFiltroAtual = statusFiltro;
            return View(tarefas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            int usuarioId = ObterUsuarioId();
            var tarefa = await _tarefaRepositorio.ObterPorIdEUsuarioAsync(id.Value, usuarioId);

            if (tarefa == null) return NotFound();

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
                await _tarefaRepositorio.AdicionarAsync(tarefa);
                return RedirectToAction(nameof(Index));
            }

            return View(tarefa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            int usuarioId = ObterUsuarioId();
            var tarefa = await _tarefaRepositorio.ObterPorIdEUsuarioAsync(id.Value, usuarioId);

            if (tarefa == null) return NotFound();

            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id) return NotFound();

            int usuarioId = ObterUsuarioId();
            var tarefaExistente = await _tarefaRepositorio.ObterPorIdEUsuarioAsync(id, usuarioId);

            if (tarefaExistente == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    tarefaExistente.Titulo = tarefa.Titulo;
                    tarefaExistente.Descricao = tarefa.Descricao;
                    tarefaExistente.Data = tarefa.Data;
                    tarefaExistente.Concluida = tarefa.Concluida;

                    await _tarefaRepositorio.AtualizarAsync(tarefaExistente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tarefaRepositorio.ExisteAsync(tarefa.Id, usuarioId))
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
            if (id == null) return NotFound();

            int usuarioId = ObterUsuarioId();
            var tarefa = await _tarefaRepositorio.ObterPorIdEUsuarioAsync(id.Value, usuarioId);

            if (tarefa == null) return NotFound();

            return View(tarefa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int usuarioId = ObterUsuarioId();
            var tarefa = await _tarefaRepositorio.ObterPorIdEUsuarioAsync(id, usuarioId);

            if (tarefa == null) return NotFound();

            await _tarefaRepositorio.RemoverAsync(tarefa);
            return RedirectToAction(nameof(Index));
        }
    }
}