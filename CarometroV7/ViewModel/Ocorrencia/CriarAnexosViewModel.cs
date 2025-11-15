using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Ocorrencia
{
    public class CriarAnexosViewModel
    {
        [Required(ErrorMessage = "Consulte o Id do Aluno no sistema")]
        [Display(Name = "AlunoId")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "Consulte o Id da turma no sistema")]
        [Display(Name = "TurmaId")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "Consulte o Id da ocorrência no sistema")]
        [Display(Name = "OcorrenciaId")]
        public int OcorrenciaId { get; set; }

        [StringLength(20, ErrorMessage = "Nome com no máximo 20 caracteres")]
        [Display(Name = "Nome do(s) anexo(s)")]
        public string? NomeDoAnexo { get; set; }

        [Required(ErrorMessage = "Anexos necessários")]
        public List<IFormFile>? Anexos { get; set; }

    }
}
