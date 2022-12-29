namespace CarometroV6.Models
{
    public class Curso
    {
        public Curso()
        {
            Turmas = new List<Turma>();
        }
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public byte Tipo { get; set; }
        public string? Cor { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public IList<Turma> Turmas { get; set; }

    }
}