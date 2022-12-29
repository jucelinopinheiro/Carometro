using System.ComponentModel.DataAnnotations;

namespace CarometroV6.ViewModel.LoginViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "NIF")]
        [Required(ErrorMessage = "Digite seu Login")]
        [Range(1, 9999999, ErrorMessage = "Campo somente números")]
        public int Login { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Digite sua senha")]
        public string? Senha { get; set; }
    }
}
