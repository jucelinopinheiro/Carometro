using CarometroV7.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarometroV7.Components
{
    public class UsuarioMenu : ViewComponent
    {
       public IViewComponentResult Invoke()
        {
            string sessaoUsario = HttpContext.Session.GetString("sessaoUsuario");
            if (string.IsNullOrEmpty(sessaoUsario)) return null;
            Usuario user = JsonConvert.DeserializeObject<Usuario>(sessaoUsario);
            return View(user);
        }
    }
}
