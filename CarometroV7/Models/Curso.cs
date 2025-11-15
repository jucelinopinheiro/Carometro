using CarometroV7.Enum;

namespace CarometroV7.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public byte Tipo { get; set; }

        public string NomeTipo
        {
            get
            {
                ETipoCurso nomeTipo = (ETipoCurso)Tipo;
                return nomeTipo.ToString();
            }
        }
        public string? Cor { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual List<Turma> Turmas { get; set;}

    }
}
