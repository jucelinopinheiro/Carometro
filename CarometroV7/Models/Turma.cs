namespace CarometroV7.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? Sigla { get; set; }
        public string? TurmaSgset { get; set; }
        public string? Classroom { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
        public string? Bag { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
        public virtual List<Matricula>? Matriculas { get; set; }
        public virtual List<Ocorrencia>? Ocorrencias { get; set; }
        public virtual List<TurmaAta>? TurmaAtas { get; set; }
    }
}
