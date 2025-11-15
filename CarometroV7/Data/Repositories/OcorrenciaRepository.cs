using CarometroV7.Data.Interfaces;
using CarometroV7.Helper;
using CarometroV7.Models;
using CarometroV7.ViewModel.Ocorrencia;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Data;

namespace CarometroV7.Data.Repositories
{
    public class OcorrenciaRepository : IOcorrencia
    {
        private readonly DataContext _context;
        private readonly ISessao _sessao;
        private readonly IAnexoOcorrencia _AnexoOcorrenciaRepository;
        private readonly IUsuario _usuarioRepository;
        private readonly IAluno _alunoRepository;
        private readonly ITurma _turmaRepository;
        private readonly Email _email;

        public OcorrenciaRepository(DataContext context, ISessao sessao, IAnexoOcorrencia anexoOcorrenciaRepository, IUsuario usuarioRepository, IAluno alunoRepository, ITurma turmaRepository, Email email)
        {
            _context = context;
            _sessao = sessao;
            _AnexoOcorrenciaRepository = anexoOcorrenciaRepository;
            _usuarioRepository = usuarioRepository;
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _email = email;
        }

        public async Task<Ocorrencia> CreatOcorrencia(CriarOcorrenciaViewModel model)
        {
            try
            {
                Usuario usuario = _sessao.BuscaSessao();
                if(usuario == null)
                {
                    usuario = new Usuario();
                    usuario.Id = 1;
                }

                var ocorrencia = new Ocorrencia
                {
                    UsuarioId = usuario.Id,
                    AlunoId = model.AlunoId,
                    Nome =model.Nome,
                    Descricao = model.Descricao,
                    TurmaId = model.TurmaId,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now

                };
                _context.Ocorrencias.Add(ocorrencia);
                await _context.SaveChangesAsync();
               

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
                        throw new Exception("0o3 - Ops! erro na extensão do anexo");
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

                    await _AnexoOcorrenciaRepository.CreateAnexoOcorrencia(anexoOcorrencia);
                }

                //email da ocorrência
                /*
                    List<Usuario> usuarios = (List<Usuario>)await _usuarioRepository.GetUsuariosNotificados();
                    var aluno = await _alunoRepository.GetAlunoById(model.AlunoId);
                    var turma = await _turmaRepository.GetTurmaById(model.TurmaId);
                    string mensagem = $@"<h2>Nova ocorrência</h2>
                                          <p><strong>Aluno: </strong> {aluno.Nome}</p>
                                          <p><strong>Curso: </strong> {turma.Curso.Descricao}</p>
                                          <p><strong>Turma: </strong> {turma.Descricao}</p>
                                          <p><strong>Ocorrido: </strong> {ocorrencia.Nome} </p>
                                          <p><strong>Ocorrência: </strong> {ocorrencia.Descricao}</p>
                                          <p><strong> Criado por: </strong>{usuario.Nome}</p>";

              
                    //enviando email de custódia

                    bool emailEnviado = _email.Enviar($"Nova ocorrência aluno(a) - {aluno.Nome}", mensagem, "", usuarios);
                    if (!emailEnviado)
                    {
                        throw new Exception("0c3 - Ops! erro ao enviar e-mail");
                    }
                */

                return ocorrencia;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> Anexar(CriarAnexosViewModel model)
        {
            try
            {
                var ocorrencia = GetOcorrenciaById(model.OcorrenciaId);
                if (ocorrencia == null)
                {
                    return false;
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
                            return false;
                        }

                        model.NomeDoAnexo = string.IsNullOrEmpty(model.NomeDoAnexo) ? "Anexo" : model.NomeDoAnexo;

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

                        _context.AnexoOcorrencias.Add(anexoOcorrencia);
                        await _context.SaveChangesAsync();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception ($"0o03 - Ops! não foi salvar ocorrência aluno Erro: {ex.Message}");
            }
        }

        public Task DeleteOcorrencia(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ocorrencia> EditOcorrencia(EditarOcorrenciaViewModel model)
        {
            var ocorrencia = await GetOcorrenciaById(model.OcorrenciaId);
            if (ocorrencia == null)
            {
                return ocorrencia;
            }

            ocorrencia.Nome = model.Nome;
            ocorrencia.Descricao = model.Descricao;
            ocorrencia.UpdateAt = DateTime.Now;
           
            _context.Update(ocorrencia);
            await _context.SaveChangesAsync();
            return ocorrencia;
        }

        public Task<IEnumerable<Ocorrencia>> GetAllOcorrencias()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ocorrencia>> GetAllOcorrenciasAlunoId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ocorrencia> GetOcorrenciaById(int id)
        {
            return await _context.Ocorrencias.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
