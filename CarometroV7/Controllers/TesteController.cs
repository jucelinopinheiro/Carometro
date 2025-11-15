using Microsoft.AspNetCore.Mvc;

namespace CarometroV7.Controllers
{
    public class TesteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
