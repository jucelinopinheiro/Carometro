using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.ViewModel.Ocorrencia;
using Microsoft.AspNetCore.Mvc;


namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class OcorrenciaController : Controller
    {
        private readonly IAluno _alunoRepositury;
        private readonly IOcorrencia _OcorrenciaRepository;

        public OcorrenciaController(IAluno alunoRepositury, IOcorrencia ocorrenciaRepository)
        {
            _alunoRepositury = alunoRepositury;
            _OcorrenciaRepository = ocorrenciaRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CriarOcorrencia(int matriculaId, int alunoId, int turmaId)
        {
            var aluno = _alunoRepositury.GetAlunoById(alunoId);

            if (aluno == null) { return View(); }

            var model = new CriarOcorrenciaViewModel
            {
                MatriculaId = matriculaId,
                AlunoId = aluno.Id,
                Nome = "",
                Descricao = "",
                TurmaId = turmaId,
                NomeDoAnexo = ""
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CriarOcorrencia(CriarOcorrenciaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _OcorrenciaRepository.CreatOcorrencia(model);
                TempData["MensagemSucesso"] = "Ocorrência criada com sucesso!";
                return Redirect($"~/Turma/AlunoDaTurma/{model.MatriculaId}");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar ocorrência Erro: {ex.Message}";
                return Redirect($"~/Turma/AlunoDaTurma/{model.MatriculaId}");
            }
        }

        public async Task<IActionResult> EditarOcorrencia(int matriculaId, int ocorrenciaId, int turmaId, int alunoId)
        {
            var ocorrencia = await _OcorrenciaRepository.GetOcorrenciaById(ocorrenciaId);
            if (ocorrencia == null)
            {
                TempData["MensagemErro"] = "Ocorrência não encotranda";
                return Redirect($"~/Turma/AlunoDaturma/{matriculaId}");
            }
            
            var editarOcorrencia = new EditarOcorrenciaViewModel
            {
                OcorrenciaId = ocorrencia.Id,
                MatriculaId = matriculaId,
                AlunoId = ocorrencia.AlunoId,
                TurmaId = ocorrencia.TurmaId,
                Nome = ocorrencia.Nome,
                Descricao = ocorrencia.Descricao

            };
            return View(editarOcorrencia);
        }

        [HttpPost]
        public async Task<IActionResult> EditarOcorrencia(EditarOcorrenciaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Erro ao salvar ocorrência";
                return Redirect($"~/Turma/AlunoDaturma/{model.AlunoId}");
            }

            try
            {
                var ocorrencia = await _OcorrenciaRepository.EditOcorrencia(model);
                if (ocorrencia == null)
                {
                    TempData["MensagemErro"] = "Erro ao salvar ocorrência";
                    return Redirect($"~/Turma/AlunoDaturma/{model.MatriculaId}");
                }

                TempData["MensagemSucesso"] = "Ocorrência atualizada!";
                return Redirect($"~/Turma/AlunoDaturma/{model.MatriculaId}");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar ocorrência Erro: {ex.Message}";
                return Redirect($"~/Turma/AlunoDaTurma/{model.MatriculaId}");
            }
            
        }

        public IActionResult CriarAnexos(int alunoId, int turmaId, int ocorrenciaId)
        {
            var ocorrencia = _OcorrenciaRepository.GetOcorrenciaById(ocorrenciaId);
            if (ocorrencia == null)
            {
                TempData["MensagemErro"] = "Erro ao localizar ocorrência";
                return Redirect($"~/Turma/AlunoDaturma/{alunoId}");
            }
            var model = new CriarAnexosViewModel
            {
                AlunoId = alunoId,
                TurmaId = turmaId,
                OcorrenciaId = ocorrenciaId,
                NomeDoAnexo = ""
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CriarAnexos(CriarAnexosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool resposta = await _OcorrenciaRepository.Anexar(model);
                
                if (!resposta) return View(model);

                return Redirect($"~/Turma/AlunoDaTurma/{model.AlunoId}");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0o03 - Ops! não foi salvar ocorrência aluno Erro: {ex.Message}";
                return Redirect($"~/Turma/AlunoDaturma/{model.AlunoId}");
            }

        }
    }
}
