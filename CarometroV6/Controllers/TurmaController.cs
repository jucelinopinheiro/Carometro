using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel.TurmaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarometroV6.Controllers
{
    [UsuarioLogado]
    public class TurmaController : Controller
    {
        public IActionResult Index([FromServices] DataContext context)
        {
            try
            {
                var turmas = context.Turmas.AsNoTracking().ToList();
                return View(turmas);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t01 - Ops! não foi possível carregar lista de turmas Erro: {ex.Message}";
                return View();
            }
        }

        public IActionResult CriarTurma([FromServices] DataContext context)
        {
            try
            {
                var cursos = context.Cursos.AsNoTracking().ToList();

                var model = new CriarTurmaViewModel
                {
                    Descricao = "",
                    Sigla = "",
                    TurmaSgset = "",
                    Classroom = "",
                    DataInicio = new DateTime(),
                    DataFim = new DateTime(),
                    CursoId = 0,
                    Cursos = new SelectList(cursos, "Id", "Descricao")
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t02 - Ops! não foi possível carregar Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult CriarTurma(CriarTurmaViewModel turmaModel, [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var cursos = context.Cursos.AsNoTracking().ToList();
                    var model = new CriarTurmaViewModel
                    {
                        Descricao = turmaModel.Descricao,
                        Sigla = turmaModel.Sigla,
                        TurmaSgset = turmaModel.TurmaSgset,
                        Classroom = turmaModel.Classroom,
                        DataInicio = turmaModel.DataInicio,
                        DataFim = turmaModel.DataFim,
                        CursoId = turmaModel.CursoId,
                        Cursos = new SelectList(cursos, "Id", "Descricao")
                    };
                    return View(model);
                }

                var turma = new Turma
                {
                    Descricao = turmaModel.Descricao,
                    Sigla = turmaModel.Sigla,
                    TurmaSgset = turmaModel.TurmaSgset.Replace(" ", ""),
                    Classroom = turmaModel.Classroom,
                    DataInicio = turmaModel.DataInicio,
                    DataFim = turmaModel.DataFim,
                    CursoId = turmaModel.CursoId,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                context.Turmas.Add(turma);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Turma criada com Sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t03 - Ops! não foi possível cadastrar essa turma Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult EditarTurma(int id, [FromServices] DataContext context)
        {
            try
            {
                var turma = context.Turmas.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (turma == null)
                {
                    TempData["MensagemErro"] = "0t04 - Ops! Turma não encontrado";
                    return RedirectToAction("Index");
                }

                var cursos = context.Cursos.AsNoTracking().ToList();
                var editorTurma = new EditarTurmaViewModel
                {
                    Id = turma.Id,
                    Descricao = turma.Descricao,
                    Sigla = turma.Sigla,
                    TurmaSgset = turma.TurmaSgset,
                    Classroom = turma.Classroom,
                    DataInicio = turma.DataInicio,
                    DataFim = turma.DataFim,
                    Ativo = turma.Ativo,
                    CursoId = turma.CursoId,
                    Cursos = new SelectList(cursos, "Id", "Descricao")

                };
                return View(editorTurma);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "0t05 - Ops! falha no servidor";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EditarTurma(EditarTurmaViewModel turmaModel, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                var cursos = context.Cursos.AsNoTracking().ToList();
                var editorTurma = new EditarTurmaViewModel
                {
                    Id = turmaModel.Id,
                    Descricao = turmaModel.Descricao,
                    Sigla = turmaModel.Sigla,
                    TurmaSgset = turmaModel.TurmaSgset,
                    Classroom = turmaModel.Classroom,
                    DataInicio = turmaModel.DataInicio,
                    DataFim = turmaModel.DataFim,
                    Ativo = turmaModel.Ativo,
                    CursoId = turmaModel.CursoId,
                    Cursos = new SelectList(cursos, "Id", "Descricao")

                };
                return View(turmaModel);
            }

            try
            {
                var turma = context.Turmas.FirstOrDefault(x => x.Id == turmaModel.Id);
                if (turma == null)
                {
                    TempData["MensagemErro"] = "0t06 - Ops! Turma não encontrado";
                    return RedirectToAction("Index");
                }

                turma.Descricao = turmaModel.Descricao;
                turma.Sigla = turmaModel.Sigla;
                turma.TurmaSgset = turmaModel.TurmaSgset.Replace(" ", "");
                turma.Classroom = turmaModel.Classroom;
                turma.DataInicio = turmaModel.DataInicio;
                turma.DataFim = turmaModel.DataFim;
                turma.Ativo = turmaModel.Ativo;
                turma.CursoId = turmaModel.CursoId;
                turma.UpdateAt = DateTime.Now;

                context.Turmas.Update(turma);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Turma atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t07 - Ops! Erro Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RemoverTurma(int id, [FromServices] DataContext context)
        {
            try
            {
                var turma = context.Turmas.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (turma == null)
                {
                    TempData["MensagemErro"] = "0t08* - Ops! Turma não encontrada";
                    return RedirectToAction("Index");
                }
                return View(turma);

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t09 - Ops! Erro no Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ExcluirTurma(int id, [FromServices] DataContext context)
        {
            try
            {
                var turma = context.Turmas.FirstOrDefault(x => x.Id == id);
                if (turma == null)
                {
                    TempData["MensagemErro"] = "0t10 - Ops! Turma não encontrado";
                    return RedirectToAction("Index");
                }

                context.Turmas.Remove(turma);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Turma excluida com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t11 - Ops! não foi excluir essa turma. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Enturmar([FromServices] DataContext context)
        {
            try
            {
                var cursos = context.Cursos.AsNoTracking().ToList();

                var model = new EnturmarViewModel
                {
                    MatriculaSgset = 0,
                    AlunoId = 0,
                    TurmaId = 0,
                    Cursos = new SelectList(cursos, "Id", "Descricao")
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0t12 - Ops! não foi possível carregar Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Consulta(int id, [FromServices] DataContext context)
        {
            try
            {
                var turma = context.Turmas.AsNoTracking().Include(x => x.Curso).Include(x => x.Matriculas).ThenInclude(x => x.Aluno).FirstOrDefault(x => x.Id == id);
                return View(turma);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0zz1 - Ops! não foi possível carregar essa turma Erro: {ex.Message}";
                return View();
            }

        }
    }
}
