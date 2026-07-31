using avaliacao_09_charles_gabriel_karina_lucas.Models;

namespace avaliacao_09_charles_gabriel_karina_lucas.Interfaces
{
    public interface ITarefaRepositorio
    {
        Task<List<Tarefa>> ObterTodasPorUsuarioAsync(int usuarioId, string? statusFiltro = null);
        Task<Tarefa?> ObterPorIdEUsuarioAsync(int id, int usuarioId);
        Task AdicionarAsync(Tarefa tarefa);
        Task AtualizarAsync(Tarefa tarefa);
        Task RemoverAsync(Tarefa tarefa);
        Task<bool> ExisteAsync(int id, int usuarioId);
    }
}