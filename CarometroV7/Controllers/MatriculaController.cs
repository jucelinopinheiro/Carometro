using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.Enum;
using CarometroV7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class MatriculaController : Controller
    {
        private readonly IMatricula _matriculaRepository;
        private readonly ITurma _turmaRepository;

        public MatriculaController(IMatricula matriculaRepository, ITurma turmaRepository)
        {
            _matriculaRepository = matriculaRepository;
            _turmaRepository = turmaRepository;
        }

        //GET Matriculas
        public async Task<IActionResult> Index()
        {
            try
            {

                var matriculas = await _matriculaRepository.GetAllMatriculas();
                if (matriculas == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar a matrícula";
                    return RedirectToAction("Index");
                }
                return View(matriculas);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops!  Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Matriculas
        public async Task<IActionResult> MatriculasCursando()
        {
            try
            {
                var matriculas = await _matriculaRepository.GetMatriculasAtivas ();
                if (matriculas == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar a matrícula";
                    return RedirectToAction("Index");
                }
                return View(matriculas);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops!  Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Matriculas
        public async Task<IActionResult> MatriculasConcluidas()
        {
            var matriculas = await _matriculaRepository.GetMatriculasConcluidas();
            return View(matriculas);
        }

        //GET Matriculas
        public async Task<IActionResult> MatriculasEvadidas()
        {
            var matriculas = await _matriculaRepository.GetMatriculasEvadidas();
            return View(matriculas);
        }

        //GET Matricula/id
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var matricula = await _matriculaRepository.GetMatriculaById(id);

                if (matricula == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar a matrícula";
                    return RedirectToAction("Index");
                }
                return View(matricula);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível localizar a matrícula Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Matricula/create
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<Turma> turmas = await _turmaRepository.GetTurmasAtivas();
                ViewBag.Turmas = new SelectList(turmas, "Id", "Descricao");
                return View();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Matricula/Edit/id
        public async Task<IActionResult>Edit(int id)
        {
            try
            {
                Matricula matricula = await _matriculaRepository.GetMatriculaById(id) ;

                if (matricula == null)
                {
                    TempData["MensagemErro"] = "Ops! Matrícula não encontrado";
                    return RedirectToAction("Index");
                }

                IEnumerable<Turma> turmas = await _turmaRepository.GetTurmasAtivas();
                ViewBag.Turmas = new SelectList(turmas, "Id", "Descricao");

                List<SelectListItem> status = EMatriculaStatus.GetValues(typeof(EMatriculaStatus))
                                         .Cast<EMatriculaStatus>()
                                         .Select(e => new SelectListItem
                                         {
                                             Text = e.ToString(),
                                             Value = ((int)e).ToString()
                                         })
                                         .ToList();
                ViewBag.Status = status;

                return View(matricula);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível editar a matrícula Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //POST Matricula/Edit

        [HttpPost]
        public async Task<IActionResult> Edit(Matricula model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<Turma> turmas = await _turmaRepository.GetTurmasAtivas();
                ViewBag.Turmas = new SelectList(turmas, "Id", "Descricao");

                List<SelectListItem> status = EMatriculaStatus.GetValues(typeof(EMatriculaStatus))
                                         .Cast<EMatriculaStatus>()
                                         .Select(e => new SelectListItem
                                         {
                                             Text = e.ToString(),
                                             Value = ((int)e).ToString()
                                         })
                                         .ToList();
                ViewBag.Status = status;
                return View(model);
            }
            try
            {
               
                await _matriculaRepository.UpdateMatricula(model);
                TempData["MensagemSucesso"] = "Turma criada com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar turma Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Matricula/Delete/5
        public async Task<IActionResult> Delete (int id)
        {
            try
            {
                var matricula = await _matriculaRepository.GetMatriculaById(id);

                if (matricula == null)
                {
                    TempData["MensagemErro"] = $"Ops! Matrícula não localizado";
                    return RedirectToAction("Index");
                }

                return View(matricula);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! Falha no servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Matricula/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _matriculaRepository.DeleteMatricula(id);
                TempData["MensagemSucesso"] = "Matrícula excluida com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível excluir a matrícula. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
