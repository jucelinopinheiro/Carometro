using CarometroV7.Data.Interfaces;
using CarometroV7.Helper;
using CarometroV7.ViewModel;
using Microsoft.AspNetCore.Mvc;
using SecureIdentity.Password;

namespace CarometroV7.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUsuario _contextUser;
        private readonly ISessao _sessao;

        public AccessController(IUsuario contextUser, ISessao sessao)
        {
            _contextUser = contextUser;
            _sessao = sessao;
        }

        public IActionResult Login()
        {
            if (_sessao.BuscaSessao() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["MensagemErro"] = "Login ou senha inválida";
                    return RedirectToAction("Login", "Access");
                }

                var userLgoin = await _contextUser.LoginGeyUserById(loginViewModel.Login);
                if (userLgoin == null)
                {
                    TempData["MensagemErro"] = "Login ou senha inválida";
                    return RedirectToAction("Login", "Access");
                }

                if (PasswordHasher.Verify(userLgoin.SenhaHash, loginViewModel.Password))
                {
                    //criando sessão
                    _sessao.CriarSessao(userLgoin);
                    return RedirectToAction("Index", "Home", new { area = "" });
                    //return RedirectToAction("Index", "Home", new { area = null });
                }

                TempData["MensagemErro"] = "Login ou senha inválida";
                return RedirectToAction("Login", "Access");

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Opsss! Não foi possível realizar seu login, Erro: {e.Message}";
                return RedirectToAction("Login", "Access");
            }

        }
        public IActionResult Logout()
        {
            _sessao.RemoverSessao();
            return RedirectToAction("Login", "Access");
        }
    }
}
