using CarometroV7.access;
using CarometroV7.Data;
using CarometroV7.Data.Interfaces;
using CarometroV7.Helper.CsvMap;
using CarometroV7.Models;
using CarometroV7.ViewModel.Aluno;
using CarometroV7.ViewModel.Matricula;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class AlunoController : Controller
    {
        private readonly IAluno _alunoRepository;
        private readonly ITurma _turmaRepository;
        private readonly IMatricula _matriculaRepository;

        public AlunoController(IAluno alunoRepository, ITurma turmaRepository, IMatricula matriculaRepository)
        {
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _matriculaRepository = matriculaRepository;
        }

        //GET Alunos
        public async Task<IActionResult> Index()
        {
            try
            {
                var alunos = await _alunoRepository.GetAllAlunos();
                return View(alunos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }
        }

        //GET Aluno/Id
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var aluno = await _alunoRepository.GetAlunoById(id);

                if (aluno == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar aluno";
                    return RedirectToAction("Index");
                }
                return View(aluno);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível localizar o aluno Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //Get Aluno/Create
        public IActionResult Create()
        {
            return View();
        }


        //POST Aluno/Create
        [HttpPost]
        public async Task<IActionResult>Create(CreateAlunoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (model.Arquivo != null)
                {
                    FileInfo fileInfo = new FileInfo(model.Arquivo.FileName);
                    string extensao = fileInfo.Extension.ToLower();
                    string[] extensoesValidas = new string[] { ".png" };

                    if (!extensoesValidas.Contains(extensao))
                    {
                        TempData["MensagemErro"] = $"0u02 - Ops! Erro na extensão do arquivo";
                        return View(model);
                    }
                }
                
                await _alunoRepository.CreateAluno(model);
                
                TempData["MensagemSucesso"] = "Aluno criado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar o aluno Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Aluno/Edit/id
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Aluno aluno = await _alunoRepository.GetAlunoById(id);

                if (aluno == null)
                {
                    TempData["MensagemErro"] = "Ops! Aluno não encontrado";
                    return RedirectToAction("Index");
                }

                var model = new EditAlunoViewModel
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    CelAluno = aluno.CelAluno,
                    EmailAluno = aluno.EmailAluno,
                    Nascimento = aluno.Nascimento,
                    Rg = aluno.Rg,
                    Cpf = aluno.Cpf,
                    Pai = aluno.Pai,
                    CelPai = aluno.CelPai,
                    Mae = aluno.Mae,
                    CelMae = aluno.CelMae,
                    Foto = aluno.Foto,
                    Pne = aluno.Pne,
                    ObsAluno = aluno.ObsAluno,
                    Ativo = aluno.Ativo
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! erro no servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        //POST Aluno/Edit
        [HttpPost]
        public async Task<IActionResult>Edit(EditAlunoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _alunoRepository.UpdateAluno(model);

                TempData["MensagemSucesso"] = "Aluno atualizado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! Erro servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        //GET Aluno/id
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _alunoRepository.GetAlunoById(id);

                if (aluno == null)
                {
                    TempData["MensagemErro"] = $"Ops! Aluno não localizado";
                    return RedirectToAction("Index");
                }
                return View(aluno);
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
                await _alunoRepository.DeleteAluno(id);
                TempData["MensagemSucesso"] = "Aluno Excluido com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Não foi possível excluir aluno, aluno desativado! {ex.Message}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível excluir o aluno. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult BuscaAluno()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscaAluno(string nome)
        {
            var alunos = await _alunoRepository.GetAlunoByName(nome);
            return View(alunos);
        }

        public IActionResult ImportAlunosCsv()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportAlunosCsv(FileUploadModel fileModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Upload\\Csv");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                FileInfo fileInfo = new FileInfo(fileModel.Arquivo.FileName);

                string extensao = fileInfo.Extension.ToLower();
                string[] extensoesValidas = new string[] { ".csv" };
                if (!extensoesValidas.Contains(extensao))
                {
                    //erro na extensão
                    TempData["MensagemErro"] = $"0a12 - Ops! Erro na extensão do arquivo";
                    return View();
                }

                //string fileName = $@"{ocorrencia.Id}_{model.NomeDoAnexo}_{fileInfo.Extension}";
                //alteração do nome do arquivo
                string fileName = $@"{DateTime.Now.ToString("yyyyMMddHHmm")}_Alunos{fileInfo.Extension}";

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    fileModel.Arquivo.CopyTo(stream);
                }

                using (var reader = new StreamReader(fileNameWithPath))
                {
                    var config = new CultureInfo("pt-BR");
                    using (var csvReader = new CsvReader(reader, config))
                    {
                        csvReader.Context.RegisterClassMap<ImportMatriculaViewModelClassMap>();
                        var registros = csvReader.GetRecords<ImportMatriculaViewModel>().ToList();

                        // declaração de turma para realizar apenas uma consulta no banco;
                        var turma = new Turma();
                        
                        foreach (ImportMatriculaViewModel model in registros)
                        {
                            // var aluno = context.Alunos.FirstOrDefault(x => x.Cpf == model.Cpf);
                            var aluno = await _alunoRepository.GetAlunoByCpf(model.Cpf);
                            if (aluno == null)
                            {
                                var fotoFileName = model.Cpf.Replace(".", "").Replace("-", "") + ".png";
                                string pathFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\fotos");

                                FileInfo file = new FileInfo(Path.Combine(pathFoto, fotoFileName));
                                if (!file.Exists)
                                {
                                    fotoFileName = "placeholder.png";
                                }

                                var novoAluno = new CreateAlunoViewModel
                                {
                                    Nome = model.Nome.ToUpperInvariant(),
                                    CelAluno = model.CelAluno,
                                    EmailAluno = model.EmailAluno,
                                    Nascimento = Convert.ToDateTime(model.Nascimento),
                                    Rg = model.Rg,
                                    Cpf = model.Cpf,
                                    Pai = model.Pai.ToUpperInvariant(),
                                    CelPai = "",
                                    Mae = model.Mae.ToUpperInvariant(),
                                    CelMae = "",
                                    Pne = (model.ObsAluno == "Não Possui" ? false : true)
                                };

                                aluno = await _alunoRepository.CreateAluno(novoAluno);


                            }

                            if (turma.TurmaSgset != model.TurmaSegset)
                            {
                                turma = await _turmaRepository.GetTurmaBySgset(model.TurmaSegset);
                            }
 
                            if (turma != null)
                            {
                                var matricula = await _matriculaRepository.GetMatriculaByIdSgset(model.MatriculaSgset);

                                if (matricula == null)
                                {
                                    var novaMatricula = new CreateMatriculaViewModel
                                    {
                                        MatriculaSgset = model.MatriculaSgset,
                                        DataMatricula = Convert.ToDateTime(model.DataMatricula),
                                        AlunoId = aluno.Id,
                                        TurmaId = turma.Id
                                    };

                                    await _matriculaRepository.CreateMatricula(novaMatricula);
                                }
                            }
                        }
                    }
                }

                TempData["MensagemSucesso"] = $"Upload do arquivo {fileName} realizado com Sucesso!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a13 - Ops! algo está errado para o upload de arquivo: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
