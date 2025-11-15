using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Usuario
{
    public class AlterarSenhaViewModel
    {
        [Display(Name = "Senha Atual")]
        [Required(ErrorMessage = "Digite sua senha atual")]
        public string? SenhaAtual { get; set; }

        [Display(Name = "Nova Senha")]
        [Required(ErrorMessage = "Digite uma nova senha")]
        public string? NovaSenha { get; set; }
    }
}
