using avaliacao_09_charles_gabriel_karina_lucas.Interfaces;
using avaliacao_09_charles_gabriel_karina_lucas.Models;
using Microsoft.EntityFrameworkCore;

namespace avaliacao_09_charles_gabriel_karina_lucas.Data
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly AppDbContext _context;

        public TarefaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> ObterTodasPorUsuarioAsync(int usuarioId, string? statusFiltro = null)
        {
            var query = _context.Tarefas.Where(t => t.UsuarioId == usuarioId);

            
            if (!string.IsNullOrEmpty(statusFiltro))
            {
                if (statusFiltro.Equals("Concluida", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(t => t.Concluida);
                }
                else if (statusFiltro.Equals("Pendente", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(t => !t.Concluida);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Tarefa?> ObterPorIdEUsuarioAsync(int id, int usuarioId)
        {
            return await _context.Tarefas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
        }

        public async Task AdicionarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id, int usuarioId)
        {
            return await _context.Tarefas.AnyAsync(t => t.Id == id && t.UsuarioId == usuarioId);
        }
    }
}