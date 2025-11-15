using CarometroV7.Models;
using CarometroV7.ViewModel.Usuario;

namespace CarometroV7.Data.Interfaces
{
    public interface IUsuario
    {
        Task<IEnumerable<Usuario>> GetUsuariosNotificados();
        Task<Usuario> LoginGeyUserById(int id);
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task<Usuario> GetUsuarioById(int id);
        Task<CreateUsuarioViewModel> CreateUsuario(CreateUsuarioViewModel model);
        Task<EditUsuarioViewModel> UpdateUsuario(EditUsuarioViewModel model);
        Task DeleteUsuario(int id);
        Task<bool> AlterarSenha(AlterarSenhaViewModel model);

    }
}