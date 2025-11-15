using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Digite seu Login")]
        [Range(1, 9999999, ErrorMessage = "Campo somente números")]
        [Display(Name = "Login")]
        public int Login { get; set; }

        [Required(ErrorMessage = "Digite sua senha")]
        [Display(Name = "Senha")]
        public string? Password { get; set; }

    }
}
