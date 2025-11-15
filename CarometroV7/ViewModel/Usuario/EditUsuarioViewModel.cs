using CarometroV7.Enum;
using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Usuario
{
    public class EditUsuarioViewModel
    {
        [Required(ErrorMessage = "O campo NIF é obrigatório")]
        [Display(Name = "NIF")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Display(Name = "Senha")]
        public string? SenhaHash { get; set; }

        [Required(ErrorMessage = "O campo Notificar é obrigatório")]
        [Display(Name = "Notificar")]
        public bool Notificar { get; set; }

        [Required(ErrorMessage = "O campo Ativo é obrigatório")]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "O campo Perfil é obrigatório")]
        [Display(Name = "Perfil")]
        public EPerfil Perfil { get; set; }

    }
}
