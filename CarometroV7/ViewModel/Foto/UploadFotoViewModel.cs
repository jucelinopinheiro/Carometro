using System.ComponentModel.DataAnnotations;
namespace CarometroV7.ViewModel.Foto
{
    public class UploadFotoViewModel
    {
        [Required(ErrorMessage = "Digite o Cpf do Aluno")]
        [StringLength(14, ErrorMessage = "Nome com no máximo 14 caracteres")]
        [Display(Name = "CPF do Aluno")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "Imagem inválida")]
        public string? Base64Image { get; set; }
    }
}
