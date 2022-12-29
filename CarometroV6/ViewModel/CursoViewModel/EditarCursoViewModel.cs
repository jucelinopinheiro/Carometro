using CarometroV6.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarometroV6.ViewModel.CursoViewModel
{
    public class EditarCursoViewModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "Id do curso obrigatório")]
        [Range(1, 9999999, ErrorMessage = "Campo somente números")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do curso")]
        [StringLength(50, ErrorMessage = "Nome com no máximo 50 caracteres")]
        [Display(Name = "Nome")]
        public string? Descricao { get; set; }
        public string? Cor { get; set; }

        [Required(ErrorMessage = "O tipo do curso é obrigatório")]
        [Display(Name = "Tipo")]
        public ETipoCurso TipoCurso { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }
        public List<SelectListItem> ListTipoCurso { get; set; }

        public EditarCursoViewModel()
        {
            ListTipoCurso = new List<SelectListItem>();
            ListTipoCurso.Add(new SelectListItem
            {
                Value = ((int)ETipoCurso.Tecnico).ToString(),
                Text = "Técnico"
            });

            ListTipoCurso.Add(new SelectListItem
            {
                Value = ((int)ETipoCurso.FIC).ToString(),
                Text = "Fic"
            });
        }
    }
}
