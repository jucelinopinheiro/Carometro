using CarometroV6.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarometroV6.ViewModel.UsuarioViewModel
{
    public class EditarUsuarioViewModel
    {
        [Display(Name = "NIF")]
        [Required(ErrorMessage = "Digite o NIF do Usuário")]
        [Range(1, 9999999, ErrorMessage = "Campo somente números")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do usuário")]
        [StringLength(80, ErrorMessage = "Nome com no máximo 80 caracteres")]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Display(Name = "Senha")]
        public string? SenhaHash { get; set; }

        [StringLength(80, ErrorMessage = "Email no máximo 80 caracteres")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Notificar")]
        public bool Notificar { get; set; }

        [Required(ErrorMessage = "Campo Perfil é obrigatório")]
        [Display(Name = "Perfil")]
        public EPerfil EPerfil { get; set; }
        public List<SelectListItem> ListPerfil { get; set; }

        public EditarUsuarioViewModel()
        {
            ListPerfil = new List<SelectListItem>();
            ListPerfil.Add(new SelectListItem
            {
                Value = ((int)EPerfil.Administrador).ToString(),
                Text = EPerfil.Administrador.ToString()
            });

            ListPerfil.Add(new SelectListItem
            {
                Value = ((int)EPerfil.Coordenador).ToString(),
                Text = EPerfil.Coordenador.ToString()
            });

            ListPerfil.Add(new SelectListItem
            {
                Value = ((int)EPerfil.Professor).ToString(),
                Text = EPerfil.Professor.ToString()
            });

            ListPerfil.Add(new SelectListItem
            {
                Value = ((int)EPerfil.Assistente).ToString(),
                Text = EPerfil.Assistente.ToString()
            });
        }
    }
}
