using System;

namespace avaliacao_09_charles_gabriel_karina_lucas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.Now;
        public bool Concluida { get; set; }
    }
}