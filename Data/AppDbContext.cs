using avaliacao_09_charles_gabriel_karina_lucas.Models;
using Microsoft.EntityFrameworkCore;

namespace avaliacao_09_charles_gabriel_karina_lucas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
    }
}