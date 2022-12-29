using CarometroV6.Data;
using CarometroV6.Enum;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel.CursoViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarometroV6.Controllers
{

    public class CursoController : Controller
    {
        [CoordenadorLogado]
        public IActionResult Index([FromServices] DataContext context)
        {
            try
            {
                var cursos = context.Cursos.AsNoTracking().ToList();
                return View(cursos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0c01 - Ops! não foi possível carregar lista de cursos Erro: {ex.Message}";
                return View();
            }
        }

        [UsuarioLogado]
        public IActionResult ListaCursos([FromServices] DataContext context)
        {
            try
            {
                var cursos = context.Cursos.AsNoTracking().Include(x => x.Turmas).ToList();
                return View(cursos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0zz01 - Ops! não foi possível carregar lista de cursos Erro: {ex.Message}";
                return View();
            }
        }

        [CoordenadorLogado]
        public IActionResult CriarCurso()
        {
            try
            {
                var model = new CriarCursoViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0c02 - Ops! não foi possível carregar Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [CoordenadorLogado]
        public IActionResult CriarCurso(CriarCursoViewModel cursoModel, [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(cursoModel);
                }

                var curso = new Curso
                {
                    Descricao = cursoModel.Descricao,
                    Tipo = (byte)cursoModel.ETipoCurso,
                    Cor = cursoModel.Cor,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                context.Cursos.Add(curso);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Curso criado com Sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0c03 - Ops! não foi possível cadastrar esse curso Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [CoordenadorLogado]
        public IActionResult EditarCurso(int id, [FromServices] DataContext context)
        {
            try
            {
                var curso = context.Cursos.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (curso == null)
                {
                    TempData["MensagemErro"] = "0c04 - Ops! Curso não encontrado";
                    return RedirectToAction("Index");
                }

                var editorCurso = new EditarCursoViewModel
                {
                    Id = curso.Id,
                    Descricao = curso.Descricao,
                    TipoCurso = (ETipoCurso)curso.Tipo,
                    Cor = curso.Cor,
                    Ativo = curso.Ativo
                };
                return View(editorCurso);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "0c05 - Ops! falha no servidor";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [CoordenadorLogado]
        public IActionResult EditarCurso(EditarCursoViewModel cursoModel, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                var editorCurso = new EditarCursoViewModel
                {
                    Id = cursoModel.Id,
                    Descricao = cursoModel.Descricao,
                    TipoCurso = cursoModel.TipoCurso,
                    Cor = cursoModel.Cor,
                    Ativo = cursoModel.Ativo
                };
                return View(cursoModel);
            }

            try
            {
                var curso = context.Cursos.FirstOrDefault(x => x.Id == cursoModel.Id);
                if (curso == null)
                {
                    TempData["MensagemErro"] = "0c06 - Ops! Curso não encontrado";
                    return RedirectToAction("Index");
                }

                curso.Descricao = cursoModel.Descricao;
                curso.Tipo = (byte)cursoModel.TipoCurso;
                curso.Cor = cursoModel.Cor;
                curso.Ativo = cursoModel.Ativo;
                curso.UpdateAt = DateTime.Now;

                context.Cursos.Update(curso);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Curso atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0c07 - Ops! Erro Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [CoordenadorLogado]
        public IActionResult RemoverCurso(int id, [FromServices] DataContext context)
        {
            try
            {
                var curso = context.Cursos.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (curso == null)
                {
                    TempData["MensagemErro"] = "0c08* - Ops! Curso não encontrado";
                    return RedirectToAction("Index");
                }
                return View(curso);

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0c09 - Ops! Erro no Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [CoordenadorLogado]
        public IActionResult ExcluirCurso(int id, [FromServices] DataContext context)
        {
            try
            {
                var curso = context.Cursos.FirstOrDefault(x => x.Id == id);
                if (curso == null)
                {
                    TempData["MensagemErro"] = "0c10 - Ops! Curso não encontrado";
                    return RedirectToAction("Index");
                }

                context.Cursos.Remove(curso);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Curso excluido com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0c11 - Ops! não foi excluir curso. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
