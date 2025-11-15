using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Curso
{
    public class EditCursoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do curso")]
        [StringLength(50, ErrorMessage = "Nome com no máximo 50 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo Tipo é obrigatório")]
        [Display(Name = "Tipo")]
        public byte Tipo { get; set; }

        [Required(ErrorMessage = "O campo Cor é obrigatório")]
        [Display(Name = "Cor")]
        public string? Cor { get; set; }

        [Required(ErrorMessage = "O campo Ativo é obrigatório")]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }
       
    }
}
