using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.Helper;
using CarometroV6.Models;
using CarometroV6.ViewModel.OcorrenciaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CarometroV6.Controllers
{
    [UsuarioLogado]
    public class OcorrenciaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CriarOcorrencia(int alunoId, int turmaId, [FromServices] DataContext context)
        {
            var aluno = context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == alunoId);

            if (aluno == null) { return View(); }

            var model = new CriarOcorrenciaViewModel
            {
                AlunoId = aluno.Id,
                Nome = "",
                Descricao = "",
                TurmaId = turmaId,
                NomeDoAnexo = ""
            };
            return View(model);

        }

        [HttpPost]
        public IActionResult CriarOcorrencia(CriarOcorrenciaViewModel model, [FromServices] DataContext context, [FromServices] Email email)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                Usuario usuario = new Usuario();
                string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuario");
                if (string.IsNullOrEmpty(sessaoUsuario))
                {
                    usuario.Id = 1;
                }
                else
                {
                    usuario = JsonConvert.DeserializeObject<Usuario>(sessaoUsuario);
                }

                var ocorrencia = new Ocorrencia
                {
                    UsuarioId = (int)usuario.Id,
                    AlunoId = model.AlunoId,
                    TurmaId = model.TurmaId,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                context.Ocorrencias.Add(ocorrencia);
                context.SaveChanges();

                if (model.Anexo != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Anexos");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    FileInfo fileInfo = new FileInfo(model.Anexo.FileName);
                    string extensao = fileInfo.Extension.ToLower();
                    string[] extensoesValidas = new string[] { ".pdf", ".jpg", ".jpeg", ".png" };
                    if (!extensoesValidas.Contains(extensao))
                    {
                        //erro na extensão
                        TempData["MensagemErro"] = $"0o3 - Ops! erro na extensão do anexo";
                        return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
                    }

                    model.NomeDoAnexo = (string.IsNullOrEmpty(model.NomeDoAnexo) ? "Anexo" : model.NomeDoAnexo);

                    string fileName = $@"{ocorrencia.Id}_{model.NomeDoAnexo}_{DateTime.Now.ToString("yyyyMMddHHmm")}{fileInfo.Extension}";
                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Anexo.CopyTo(stream);
                    }

                    var anexoOcorrencia = new AnexoOcorrencia
                    {
                        OcorrenciaId = ocorrencia.Id,
                        Descricao = model.NomeDoAnexo,
                        UrlAnexo = @$"./Files/Anexos/{fileName}",
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now
                    };

                    context.AnexoOcorrencias.Add(anexoOcorrencia);
                    context.SaveChanges();


                }

                //email da ocorrência
                List<Usuario> usuarios = context.Usuarios.AsNoTracking().Where(x => x.Notificar == true).ToList();
                var aluno = context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == model.AlunoId);
                var turma = context.Turmas.AsNoTracking().Include(x => x.Curso).FirstOrDefault(x => x.Id == model.TurmaId);
                string mensagem = $@"<h2>Nova ocorrência</h2>
                                      <p><strong>Aluno: </strong> {aluno.Nome}</p>
                                      <p><strong>Curso: </strong> {turma.Curso.Descricao}</p>
                                      <p><strong>Turma: </strong> {turma.Descricao}</p>
                                      <p><strong>Ocorrido: </strong> {ocorrencia.Nome} </p>
                                      <p><strong>Ocorrência: </strong> {ocorrencia.Descricao}</p>";

                //enviando email de custódia
                bool emailEnviado = email.Enviar($"Nova ocorrência aluno(a) - {aluno.Nome}", mensagem, "", usuarios);
                if (!emailEnviado)
                {
                    TempData["MensagemErro"] = $"0c3 - Ops! erro ao enviar e-mail";
                    return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
                }

                TempData["MensagemSucesso"] = "Ocorrência Salva";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });


            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0o03 - Ops! não foi salvar ocorrência aluno Erro: {ex.Message}";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
            }
        }

        public IActionResult EditarOcorrencia(int ocorrenciaId, int turmaId, int alunoId, [FromServices] DataContext context)
        {
            var ocoorencia = context.Ocorrencias.AsNoTracking().FirstOrDefault(x => x.Id == ocorrenciaId);
            if (ocoorencia == null)
            {
                TempData["MensagemErro"] = "Ocorrência não encotranda";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = alunoId, turmaId = turmaId });
            }
            var editorOcorrencia = new EditarOcorrenciaViewModel
            {
                OcorrenciaId = ocorrenciaId,
                AlunoId = alunoId,
                TurmaId = turmaId,
                Nome = ocoorencia.Nome,
                Descricao = ocoorencia.Descricao
            };
            return View(editorOcorrencia);

        }

        [HttpPost]
        public IActionResult EditarOcorrencia(EditarOcorrenciaViewModel model, [FromServices] DataContext context)
        {
            try
            {
                var ocorrencia = context.Ocorrencias.AsNoTracking().FirstOrDefault(x => x.Id == model.OcorrenciaId);
                if (ocorrencia == null)
                {
                    TempData["MensagemErro"] = "Ocorrência não encotranda";
                    return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
                }

                ocorrencia.Nome = model.Nome;
                ocorrencia.Descricao = model.Descricao;
                ocorrencia.UpdateAt = DateTime.Now;

                context.Update(ocorrencia);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Ocorrência Salva";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0zz07 - Ops! Erro Servidor. Erro: {ex.Message}";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
            }

        }

        public IActionResult CriarAnexos(int alunoId, int turmaId, int ocorrenciaId, [FromServices] DataContext context)
        {
            var ocorrencia = context.Ocorrencias.AsNoTracking().FirstOrDefault(x => x.Id == ocorrenciaId);
            if (ocorrencia == null)
            {
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = alunoId, turmaId = turmaId });
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
        public IActionResult CriarAnexos(CriarAnexosViewModel model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var ocorrencia = context.Ocorrencias.AsNoTracking().FirstOrDefault(x => x.Id == model.OcorrenciaId);
                if (ocorrencia == null)
                {
                    TempData["MensagemErro"] = "Ocorrência não encotranda";
                    return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
                }
                if (model.Anexos.Count > 0)
                {
                    int i = 0;
                    foreach (var anexo in model.Anexos)
                    {
                        i++;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Anexos");

                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        FileInfo fileInfo = new FileInfo(anexo.FileName);
                        string extensao = fileInfo.Extension.ToLower();
                        string[] extensoesValidas = new string[] { ".pdf", ".jpg", ".jpeg", ".png" };
                        if (!extensoesValidas.Contains(extensao))
                        {
                            //erro na extensão
                            TempData["MensagemErro"] = $"0zz3 - Ops! erro na extensão do anexo";
                            return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
                        }

                        model.NomeDoAnexo = (string.IsNullOrEmpty(model.NomeDoAnexo) ? "Anexo" : model.NomeDoAnexo);

                        string fileName = $@"{model.OcorrenciaId}_{model.NomeDoAnexo}{i}_{DateTime.Now.ToString("yyyyMMddHHmm")}{fileInfo.Extension}";
                        string fileNameWithPath = Path.Combine(path, fileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            anexo.CopyTo(stream);
                        }

                        var anexoOcorrencia = new AnexoOcorrencia
                        {
                            OcorrenciaId = model.OcorrenciaId,
                            Descricao = (string.IsNullOrEmpty(model.NomeDoAnexo) ? "Anexo" : model.NomeDoAnexo),
                            UrlAnexo = @$"./Files/Anexos/{fileName}",
                            CreateAt = DateTime.Now,
                            UpdateAt = DateTime.Now
                        };

                        context.AnexoOcorrencias.Add(anexoOcorrencia);
                        context.SaveChanges();
                    }

                    TempData["MensagemSucesso"] = "Ocorrência Salva com anexo";
                    return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
                }

                TempData["MensagemErro"] = "Nenhum anexo enviado";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0o03 - Ops! não foi salvar ocorrência aluno Erro: {ex.Message}";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = model.AlunoId, turmaId = model.TurmaId });
            }

        }

        public IActionResult RemoverOcorrencia(int ocorrenciaId, int turmaId, int alunoId, [FromServices] DataContext context)
        {
            try
            {
                var ocorrencia = context.Ocorrencias.AsNoTracking().FirstOrDefault(x => x.Id == ocorrenciaId);
                if (ocorrencia == null)
                {
                    TempData["MensagemErro"] = "0a10 - Ops! Ocorrência não encontrada";
                    return RedirectToAction("Detalhe", "Aluno", new { alunoId = alunoId, turmaId = turmaId });
                }
                return View(ocorrencia);

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a09 - Ops! Erro no Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ExcluirOcorrencia(int ocorrenciaId, int turmaId, int alunoId, [FromServices] DataContext context)
        {
            try
            {
                var ocorrencia = context.Ocorrencias.AsNoTracking().FirstOrDefault(x => x.Id == ocorrenciaId);
                if (ocorrencia == null)
                {
                    TempData["MensagemErro"] = "0a10 - Ops! Ocorrência não encontrada";
                    return RedirectToAction("Detalhe", "Aluno", new { alunoId = alunoId, turmaId = turmaId });
                }

                var anexos = context.AnexoOcorrencias.AsNoTracking().Where(x => x.OcorrenciaId == ocorrencia.Id).ToList();
                if (anexos != null)
                {
                    foreach (AnexoOcorrencia anexo in anexos)
                    {
                        var arr = anexo.UrlAnexo.Split('/');
                        var fileName = arr[arr.Length - 1];
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Anexos");

                        FileInfo file = new FileInfo(Path.Combine(path, fileName));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        context.AnexoOcorrencias.Remove(anexo);
                        context.SaveChanges();
                    }
                }


                context.Ocorrencias.Remove(ocorrencia);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Ocorrência excluida com sucesso";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = alunoId, turmaId = turmaId });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0a11 - Ops! não foi excluir essa ocorrência. Erro: {ex.Message}";
                return RedirectToAction("Detalhe", "Aluno", new { alunoId = alunoId, turmaId = turmaId });
            }
        }


    }

}
