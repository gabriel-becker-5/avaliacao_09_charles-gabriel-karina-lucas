
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using avaliacao_09_charles_gabriel_karina_lucas.Models;

public class TarefasController : Controller
{
    private readonly avaliacao_09_charles_gabriel_karina_lucasContext _context;

    public TarefasController(avaliacao_09_charles_gabriel_karina_lucasContext context)
    {
        _context = context;
    }

    
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Tarefa.ToListAsync());
    }

  
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefa
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tarefa == null)
        {
            return NotFound();
        }

        return View(tarefa);
    }

    // GET: TAREFAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TAREFAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Data,Concluida")] Tarefa tarefa)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tarefa);
    }

    // GET: TAREFAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefa.FindAsync(id);
        if (tarefa == null)
        {
            return NotFound();
        }
        return View(tarefa);
    }

    // POST: TAREFAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Titulo,Descricao,Data,Concluida")] Tarefa tarefa)
    {
        if (id != tarefa.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tarefa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(tarefa.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tarefa);
    }

    // GET: TAREFAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefa
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tarefa == null)
        {
            return NotFound();
        }

        return View(tarefa);
    }

    // POST: TAREFAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var tarefa = await _context.Tarefa.FindAsync(id);
        if (tarefa != null)
        {
            _context.Tarefa.Remove(tarefa);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TarefaExists(int? id)
    {
        return _context.Tarefa.Any(e => e.Id == id);
    }
}
