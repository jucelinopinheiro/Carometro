using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Matricula
{
    public class CreateMatriculaViewModel
    {

        [Display(Name = "Matrícula SGSET")]
        [Required(ErrorMessage = "Matrícula do SGSET")]
        [Range(1, 9999999, ErrorMessage = "Campo somente números")]
        public int MatriculaSgset { get; set; }

        [Required(ErrorMessage = "Data da matrícula")]
        [Display(Name = "Data da matrícula")]
        public DateTime DataMatricula { get; set; }

        [Required(ErrorMessage = "Id do aluno")]
        [Display(Name = "Aluno")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "Id da turma")]
        [Display(Name = "Turma")]
        public int TurmaId { get; set; }

    }
}
