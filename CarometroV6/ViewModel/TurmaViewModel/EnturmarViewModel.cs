using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarometroV6.ViewModel.TurmaViewModel
{
    public class EnturmarViewModel
    {
        [Required(ErrorMessage = "O campo matrícula é obrigatório")]
        [Range(1, 9999999999, ErrorMessage = "Campo somente números")]
        [Display(Name = "Matrícula")]
        public int MatriculaSgset { get; set; }

        [Required(ErrorMessage = "O campo matrícula é obrigatório")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "O campo matrícula é obrigatório")]
        public int TurmaId { get; set; }
        public SelectList? Cursos { get; set; }
    }
}