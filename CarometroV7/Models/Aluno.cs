namespace CarometroV7.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? CelAluno { get; set; }
        public string? EmailAluno { get; set; }
        public DateTime Nascimento { get; set; }
        public string? Rg { get; set; }
        public string? Cpf { get; set; }
        public string? Pai { get; set; }
        public string? CelPai { get; set; }
        public string? Mae { get; set; }
        public string? CelMae { get; set; }
        public string? Foto { get; set; }
        public bool Pne { get; set; }
        public string? ObsAluno { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual List<Matricula>? Matriculas { get; set; }
        public virtual List<Ocorrencia>? Ocorrencias { get; set; }

    }
}
