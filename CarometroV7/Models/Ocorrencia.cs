namespace CarometroV7.Models
{
    public class Ocorrencia
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int AlunoId { get; set; }
        public Aluno? Aluno { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual List<AnexoOcorrencia>? Anexos { get; set; }
    }
}
