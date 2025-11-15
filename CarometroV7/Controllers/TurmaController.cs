using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.Models;
using CarometroV7.ViewModel.Turma;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class TurmaController : Controller
    {
        private readonly ITurma _turmaRepository;
        private readonly ICurso _cursoRepository;
        private readonly IMatricula _matriculaRepository;
        public TurmaController(ITurma turmaRepository, ICurso cursoRepository, IMatricula matriculaRepository)
        {
            _turmaRepository = turmaRepository;
            _cursoRepository = cursoRepository;
            _matriculaRepository = matriculaRepository;
        }

        public async Task<IActionResult> AlunoDaTurma(int id)
        {
            try
            {
                var matricula = await _matriculaRepository.GetMatriculaAluno(id);
                return View(matricula);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }
        }
        public async Task<IActionResult> AlunosDaTurma(int id)
        {
            try
            { 
                var turma = await _turmaRepository.GetAlunosDaTurma(id);
                return View(turma);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var turmas = await _turmaRepository.GetTurmasDesc();
                return View(turmas);
            }
            catch(Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }
        }

        //retorna somente lista com turmas que estão ativas
        public async Task<IActionResult> TurmasAtiva()
        {
            try
            {
                var turmas = await _turmaRepository.GetTurmasAtivas();
                return View(turmas);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }
        }

        //GET Turma/id
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var turma = await _turmaRepository.GetTurmaById(id);

                if (turma == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar a turma";
                    return RedirectToAction("Index");
                }
                return View(turma);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível localizar a turma Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Turma/create
        [UsuarioLogado]
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<Curso> cursos = await _cursoRepository.GetCursos();
                ViewBag.Cursos = new SelectList(cursos, "Id", "Descricao");
                return View();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //POST Turma/create
        [UsuarioLogado]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTurmaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<Curso> cursos = await _cursoRepository.GetCursos();
                ViewBag.Cursos = new SelectList(cursos, "Id", "Descricao");
                return View(model);
            }
            try
            {
                if(model.Arquivo != null)
                {
                    FileInfo fileInfo = new FileInfo(model.Arquivo.FileName);
                    string extensao = fileInfo.Extension.ToLower();
                    string[] extensoesValidas = new string[] { ".png" };

                    if (!extensoesValidas.Contains(extensao))
                    {
                        //erro na extensão
                        IEnumerable<Curso> cursos = await _cursoRepository.GetCursos();
                        ViewBag.Cursos = new SelectList(cursos, "Id", "Descricao");
                        TempData["MensagemErro"] = $"0u02 - Ops! Erro na extensão do arquivo";
                        return View();
                    }
                }
                await _turmaRepository.CreateTurma(model);
                TempData["MensagemSucesso"] = "Turma criada com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar turma Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Turma/Edit/id
        [UsuarioLogado]
        public async Task<IActionResult>Edit(int id)
        {
            try
            {
                Turma turma = await _turmaRepository.GetTurmaById(id);
                
                if (turma == null)
                {
                    TempData["MensagemErro"] = "Ops! Turma não encontrada";
                    return RedirectToAction("Index");
                }

                var model = new EditTurmaViewModel
                {
                   Id = turma.Id,
                   Descricao = turma.Descricao,
                   Sigla = turma.Sigla,
                   TurmaSgset = turma.TurmaSgset,
                   Classroom = turma.Classroom,
                   DataInicio = turma.DataInicio,
                   DataFim = turma.DataFim,
                   Ativo = turma.Ativo,
                   Bag =turma.Bag,
                   CursoId = turma.CursoId
                };

                IEnumerable<Curso> cursos = await _cursoRepository.GetCursos();
                ViewBag.Cursos = new SelectList(cursos, "Id", "Descricao");
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível editar a turma Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //POST Turma/Edit
        [UsuarioLogado]
        [HttpPost]
        public async Task<IActionResult>Edit(EditTurmaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _turmaRepository.UpdateTurma(model);
                TempData["MensagemSucesso"] = "Turma atualizada com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível atualizar a turma Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Turma/Delete/5
        [UsuarioLogado]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var turma = await _turmaRepository.GetTurmaById(id);

                if (turma == null)
                {
                    TempData["MensagemErro"] = $"Ops! Turma não localizado";
                    return RedirectToAction("Index");
                }

                return View(turma);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! Falha no servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Turma/Delete/5
        [UsuarioLogado]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _turmaRepository.DeleteTurma(id);
                TempData["MensagemSucesso"] = "Turma excluida com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Não foi possível excluir a turma, turma desativada! {ex.Message}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível excluir a turma. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //Get CreateAta
        public async Task<IActionResult> CreateAta(int id)
        {
            try
            {
                var model = new CreateTurmaAtaViewModel()
                {
                    TurmaId = id
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! Falha no servidor Erro: {ex.Message}";
                return RedirectToAction("AlunosDaTurma" , id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAta(CreateTurmaAtaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _turmaRepository.CreateAta(model);
               return RedirectToAction("AlunosDaTurma", new {id = model.TurmaId });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível atualizar a turma Erro: {ex.Message}";
                return RedirectToAction("AlunosDaTurma", new { id = model.TurmaId });
            }

        }
    }
}
