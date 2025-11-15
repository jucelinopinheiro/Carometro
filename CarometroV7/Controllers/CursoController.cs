using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.Enum;
using CarometroV7.Filters;
using CarometroV7.Models;
using CarometroV7.ViewModel.Curso;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class CursoController : Controller
    {
        private readonly ICurso _cursoRepository;

        public CursoController(ICurso cursoRepositoty)
        {
            _cursoRepository = cursoRepositoty;
        }

        //Get cursos
        public async Task<IActionResult> Index()
        {
            try
            {
                var cursos = await _cursoRepository.GetCursos();
                return View(cursos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }
            
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var curso = await _cursoRepository.GetCursoById(id);

                if (curso == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar o curso";
                    return RedirectToAction("Index");
                }

                return View(curso);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível localizar o curso Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Curso/Create
        [UsuarioLogado]
        public async Task<IActionResult> Create()
        {
            try
            {
                List<SelectListItem> tipos = ETipoCurso.GetValues(typeof(ETipoCurso))
                                         .Cast<ETipoCurso>()
                                         .Select(e => new SelectListItem
                                         {
                                             Text = e.ToString(),
                                             Value = ((int)e).ToString()
                                         })
                                         .ToList();
                ViewBag.Tipos = tipos;
                return View();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível carregar página: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Curso/Create
        [UsuarioLogado]
        [HttpPost]
        public async Task<IActionResult>Create(CreateCursoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _cursoRepository.CreateCurso(model);
                TempData["MensagemSucesso"] = "Curso criado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar curso Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Curso/Edit/5
        [UsuarioLogado]
        public async Task<IActionResult>Edit(int id)
        {
            try
            {
                Curso curso = await _cursoRepository.GetCursoById(id);
                if (curso == null)
                {
                    TempData["MensagemErro"] = "Ops! Curso não encontrado";
                    return RedirectToAction("Index");
                }

                var model = new EditCursoViewModel
                {
                    Id = curso.Id,
                    Descricao = curso.Descricao,
                    Tipo = curso.Tipo,
                    Cor = curso.Cor,
                    Ativo = curso.Ativo

                };

                List<SelectListItem> tipos = ETipoCurso.GetValues(typeof(ETipoCurso))
                                          .Cast<ETipoCurso>()
                                          .Select(e => new SelectListItem
                                          {
                                              Text = e.ToString(),
                                              Value = ((int)e).ToString()
                                          })
                                          .ToList();
                ViewBag.Tipos = tipos;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível editar o curso Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Post: Curso/Edit/5
        [UsuarioLogado]
        [HttpPost]
        public async Task<IActionResult>Edit(EditCursoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _cursoRepository.UpdateCurso(model);
                TempData["MensagemSucesso"] = "Curso atualizado com Sucesso!";
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível atualizar o Curso Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Curso/Delete/5
        [UsuarioLogado]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var curso = await _cursoRepository.GetCursoById(id);
               
                if (curso == null)
                {
                    TempData["MensagemErro"] = $"Ops! Curso não localizado";
                    return RedirectToAction("Index");
                }

                return View(curso);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! Falha no servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        // POST: Curso/Delete/5
        [UsuarioLogado]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _cursoRepository.DeleteCurso(id);
                TempData["MensagemSucesso"] = "Curso excluido com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Não foi possível excluir o curso, curso desativado! {ex.Message}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível excluir o curso. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
