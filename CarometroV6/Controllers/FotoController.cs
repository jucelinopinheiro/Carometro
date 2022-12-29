using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.ViewModel.FotoViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CarometroV6.Controllers
{
    [UsuarioLogado]
    public class FotoController : Controller
    {
        public IActionResult Index(string? cpf)
        {
            try
            {
                var model = new UploadFotoViewModel();
                model.Cpf = cpf;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0f01 - Ops! não foi possível carregar Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UploadFoto(UploadFotoViewModel fotoModel, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                return View(fotoModel);
            }
            try
            {
                var fileName = $"{fotoModel.Cpf.Replace(".", "").Replace("-", "").Replace(" ", "")}.png";

                //dependendo a lib,a  imagem sempre vem com algumas informações a mais que
                //precisamos remover, então usamos a expressão abaixo
                var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(fotoModel.Base64Image, "");

                //transformando em bytes
                var bytes = Convert.FromBase64String(data);

                //methodo WriteallBytes que escreve tudo no disco
                System.IO.File.WriteAllBytes($"wwwroot/Files/Fotos/{fileName}", bytes);

                var aluno = context.Alunos.FirstOrDefault(x => x.Cpf == fotoModel.Cpf);
                if (aluno != null)
                {
                    aluno.Foto = fileName;
                    context.Alunos.Update(aluno);
                    context.SaveChanges();
                }

                TempData["MensagemSucesso"] = "Upload realizado com sucesso!";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0f02 - Ops! não foi possível fazer o Upload da foto. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }

        }
    }
}
