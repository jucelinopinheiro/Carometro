namespace CarometroV6.Models
{
    public class Turma
    {
        public Turma()
        {
            Matriculas = new List<Matricula>();
            Ocorrencias = new List<Ocorrencia>();
        }
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? Sigla { get; set; }
        public string? TurmaSgset { get; set; }
        public string? Classroom { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
        public List<Matricula> Matriculas { get; set; }
        public List<Ocorrencia> Ocorrencias { get; set; }

    }
}