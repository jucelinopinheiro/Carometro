using CarometroV6.Data;
using CarometroV6.Helper;
using CarometroV6.ViewModel.LoginViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace CarometroV6.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index([FromServices] Sessao sessao)
        {
            var usuario = sessao.BuscaSessao();
            if (usuario == null) return View();
            return RedirectToAction("ListaCursos", "Curso");
        }

        [HttpPost]
        public IActionResult Entrar(LoginViewModel loginModel, [FromServices] DataContext context, [FromServices] Sessao sessao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = context.Usuarios.AsNoTracking().FirstOrDefault(x => x.Id == loginModel.Login);
                    if (usuario == null)
                    {
                        TempData["MensagemErro"] = $"Opsss! Usuário inválido";
                        return RedirectToAction("Index", "Login");
                    }

                    if (PasswordHasher.Verify(usuario.SenhaHash, loginModel.Senha))
                    {
                        //usuario.SenhaHash == loginModel.Senha
                        //criando sessão
                        sessao.CriarSessao(usuario);
                        return RedirectToAction("ListaCursos", "Curso");
                    }
                    TempData["MensagemErro"] = $"Opsss! Senha inválida";
                    return RedirectToAction("Index", "Login");
                }

                TempData["MensagemErro"] = $"Opsss! Não foi possível realizar seu login";
                return RedirectToAction("Index", "Login");


            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Opsss! Não foi possível realizar seu login, Erro: {e.Message}";
                return RedirectToAction("Index", "Login");
            }

        }
        public IActionResult Sair([FromServices] Sessao sessao)
        {
            sessao.RemoverSessao();
            return RedirectToAction("Index", "Login");
        }
    }
}
