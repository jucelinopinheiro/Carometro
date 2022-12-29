using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel.AlunoViewModel;
using CarometroV6.ViewModel.MatriculaViewModel;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarometroV6.Controllers
{
    [UsuarioLogado]
    public class AlunoController : Controller
    {
        public IActionResult Index([FromServices] DataContext context)
        {
            try
            {
                var alunos = context.Alunos.AsNoTracking().ToList();
                return View(alunos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a01 - Ops! não foi possível carregar lista de alunos Erro: {ex.Message}";
                return View();
            }
        }

        public IActionResult CriarAluno()
        {
            try
            {
                var model = new CriarAlunoViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a02 - Ops! não foi possível carregar Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult CriarAluno(CriarAlunoViewModel alunoModel, [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(alunoModel);
                }

                var fotoFileName = alunoModel.Cpf.Replace(".", "").Replace("-", "") + ".png";
                string pathFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\fotos");

                FileInfo file = new FileInfo(Path.Combine(pathFoto, fotoFileName));
                if (!file.Exists)
                {
                    fotoFileName = "placeholder.png";
                }

                var aluno = new Aluno
                {
                    Nome = alunoModel.Nome,
                    CelAluno = alunoModel.CelAluno,
                    EmailAluno = alunoModel.EmailAluno,
                    Nascimento = Convert.ToDateTime(alunoModel.Nascimento),
                    Rg = alunoModel.Rg,
                    Cpf = alunoModel.Cpf,
                    Pai = alunoModel.Pai,
                    CelPai = alunoModel.CelPai,
                    Mae = alunoModel.Mae,
                    CelMae = alunoModel.CelMae,
                    Foto = fotoFileName,
                    Pne = alunoModel.Pne,
                    ObsAluno = alunoModel.ObsAluno,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                context.Alunos.Add(aluno);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Aluno salvo com Sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a03 - Ops! não foi possível cadastrar esse aluno Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult EditarAluno(int id, [FromServices] DataContext context)
        {
            try
            {
                var aluno = context.Alunos.AsNoTracking().Include(x => x.Ocorrencias).FirstOrDefault(x => x.Id == id);
                if (aluno == null)
                {
                    TempData["MensagemErro"] = "0a04 - Ops! Aluno não encontrado";
                    return RedirectToAction("Index");
                }

                var editorAluno = new EditarAlunoViewModel
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
                    ObsAluno = aluno.ObsAluno
                };
                return View(editorAluno);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "0a05 - Ops! falha no servidor";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EditarAluno(EditarAlunoViewModel alunoModel, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                var editorAluno = new EditarAlunoViewModel
                {
                    Id = alunoModel.Id,
                    Nome = alunoModel.Nome,
                    CelAluno = alunoModel.CelAluno,
                    EmailAluno = alunoModel.EmailAluno,
                    Nascimento = alunoModel.Nascimento,
                    Rg = alunoModel.Rg,
                    Cpf = alunoModel.Cpf,
                    Pai = alunoModel.Pai,
                    CelPai = alunoModel.CelPai,
                    Mae = alunoModel.Mae,
                    CelMae = alunoModel.CelMae,
                    Pne = alunoModel.Pne,
                    ObsAluno = alunoModel.ObsAluno

                };
                return View(editorAluno);
            }

            try
            {
                var aluno = context.Alunos.FirstOrDefault(x => x.Id == alunoModel.Id);
                if (aluno == null)
                {
                    TempData["MensagemErro"] = "0a06 - Ops! Aluno não encontrado";
                    return RedirectToAction("Index");
                }

                aluno.Id = alunoModel.Id;
                aluno.Nome = alunoModel.Nome;
                aluno.CelAluno = alunoModel.CelAluno;
                aluno.EmailAluno = alunoModel.EmailAluno;
                aluno.Nascimento = alunoModel.Nascimento;
                aluno.Rg = alunoModel.Rg;
                aluno.Cpf = alunoModel.Cpf;
                aluno.Pai = alunoModel.Pai;
                aluno.CelPai = alunoModel.CelPai;
                aluno.Mae = alunoModel.Mae;
                aluno.CelMae = alunoModel.CelMae;
                aluno.Pne = alunoModel.Pne;
                aluno.ObsAluno = alunoModel.ObsAluno;
                aluno.UpdateAt = DateTime.Now;

                context.Alunos.Update(aluno);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Aluno atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a07 - Ops! Erro Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RemoverAluno(int id, [FromServices] DataContext context)
        {
            try
            {
                var aluno = context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (aluno == null)
                {
                    TempData["MensagemErro"] = "0a08* - Ops! Aluno não encontrado";
                    return RedirectToAction("Index");
                }
                return View(aluno);

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a09 - Ops! Erro no Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ExcluirAluno(int id, [FromServices] DataContext context)
        {
            try
            {
                var aluno = context.Alunos.FirstOrDefault(x => x.Id == id);
                if (aluno == null)
                {
                    TempData["MensagemErro"] = "0a10 - Ops! Aluno não encontrado";
                    return RedirectToAction("Index");
                }

                context.Alunos.Remove(aluno);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Aluno excluido com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a11 - Ops! não foi excluir esse aluno. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ImportAlunosCsv()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportAlunosCsv(FileUploadModel fileModel, [FromServices] DataContext context)
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

                string fileName = "Alunos" + fileInfo.Extension;
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
                        foreach (ImportMatriculaViewModel model in registros)
                        {
                            var aluno = context.Alunos.FirstOrDefault(x => x.Cpf == model.Cpf);
                            if (aluno == null)
                            {
                                var fotoFileName = model.Cpf.Replace(".", "").Replace("-", "") + ".png";
                                string pathFoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\fotos");

                                FileInfo file = new FileInfo(Path.Combine(pathFoto, fotoFileName));
                                if (!file.Exists)
                                {
                                    fotoFileName = "placeholder.png";
                                }

                                aluno = new Aluno
                                {
                                    Nome = model.Nome.ToUpperInvariant(),
                                    CelAluno = model.CelAluno,
                                    EmailAluno = model.EmailAluno,
                                    Nascimento = Convert.ToDateTime(model.Nascimento),
                                    Rg = model.Rg,
                                    Cpf = model.Cpf,
                                    Pai = model.Pai.ToUpperInvariant(),
                                    Mae = model.Mae.ToUpperInvariant(),
                                    Foto = fotoFileName,
                                    Pne = (model.ObsAluno == "Não Possui" ? false : true),
                                    ObsAluno = (model.ObsAluno == "Não Possui" ? "" : model.ObsAluno),
                                    CreateAt = DateTime.Now,
                                    UpdateAt = DateTime.Now
                                };

                                context.Alunos.Add(aluno);
                                context.SaveChanges();
                            }

                            var turma = context.Turmas.AsNoTracking().FirstOrDefault(x => x.TurmaSgset == model.TurmaSegset);
                            if (turma != null)
                            {
                                var matricula = context.Matriculas.AsNoTracking().FirstOrDefault(x => x.MatriculaSgset == model.MatriculaSgset);
                                if (matricula == null)
                                {
                                    matricula = new Matricula
                                    {
                                        MatriculaSgset = model.MatriculaSgset,
                                        DataMatricula = Convert.ToDateTime(model.DataMatricula),
                                        AlunoId = aluno.Id,
                                        TurmaId = turma.Id,
                                        CreateAt = DateTime.Now,
                                        UpdateAt = DateTime.Now
                                    };
                                    context.Matriculas.Add(matricula);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }

                TempData["MensagemSucesso"] = $"Upload do arquivo {fileName} realizado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a13 - Ops! algo está errado para o upload de arquivo: {ex.Message}";
                return View();
            }
        }

        public IActionResult Detalhe(int alunoId, int turmaId, [FromServices] DataContext context)
        {
            try
            {
                var aluno = context.Alunos.AsNoTracking().Include(x => x.Matriculas).ThenInclude(m => m.Turma).Include(x => x.Ocorrencias).ThenInclude(o => o.Anexos).Include(x => x.Matriculas).ThenInclude(m => m.Turma).ThenInclude(t => t.Curso).Include(x => x.Ocorrencias).ThenInclude(x => x.Usuario).Include(x => x.Ocorrencias).ThenInclude(x => x.Turma).FirstOrDefault(x => x.Id == alunoId);
                var turma = context.Turmas.AsNoTracking().Include(x => x.Curso).FirstOrDefault(x => x.Id == turmaId);
                if (aluno == null && turma == null)
                {
                    TempData["MensagemErro"] = "0a14 - Ops! Aluno ou turma não localizada";
                    return RedirectToAction("Index");
                }
                var model = new AlunoDetalhadoViewModel
                {
                    Aluno = aluno,
                    Turma = turma
                };
                return View(model);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "0a15 - Ops! falha no servidor";
                return RedirectToAction("Index");
            }
        }


    }

}
