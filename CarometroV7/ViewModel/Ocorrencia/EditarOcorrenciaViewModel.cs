using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Ocorrencia
{
    public class EditarOcorrenciaViewModel
    {
        [Required(ErrorMessage = "Consulte o Id da ocorrência")]
        [Display(Name = "Id")]
        public int OcorrenciaId { get; set; }

        [Required(ErrorMessage = "Consulte o Id da matricula")]
        [Display(Name = "MatriculaId")]
        public int MatriculaId { get; set; }

        [Required(ErrorMessage = "Consulte o Id do Aluno no sistema")]
        [Display(Name = "Id")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "Consulte o Id do Aluno no sistema")]
        [Display(Name = "TurmaId")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "Digite uma prévia da ocorrência")]
        [StringLength(20, ErrorMessage = "Nome com no máximo 20 caracteres")]
        [Display(Name = "Ocorrido")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Digite a ocorrência")]
        [Display(Name = "Ocorrência")]
        public string? Descricao { get; set; }
    }
}
