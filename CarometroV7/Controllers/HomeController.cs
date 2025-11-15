using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class HomeController : Controller
    {
        private readonly ICurso _cursoRepository;

        public HomeController(ICurso cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cursos = await _cursoRepository.GetCursosAtivos();
                return View(cursos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0zz01 - Ops! não foi possível carregar lista de cursos Erro: {ex.Message}";
                return View();
            }
        }

    }
}
