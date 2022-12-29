namespace CarometroV6.Models
{
    public class Matricula
    {
        public int Id { get; set; }
        public int MatriculaSgset { get; set; }
        public DateTime DataMatricula { get; set; }
        public int AlunoId { get; set; }
        public Aluno? Aluno { get; set; }
        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
