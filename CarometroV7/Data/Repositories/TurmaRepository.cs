using CarometroV7.Data.Interfaces;
using CarometroV7.Enum;
using CarometroV7.Helper;
using CarometroV7.Models;
using CarometroV7.ViewModel.Turma;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace CarometroV7.Data.Repositories
{
    public class TurmaRepository : ITurma
    {
        private readonly DataContext _context;
        private readonly ISessao _sessao;

        public TurmaRepository(DataContext context, ISessao sessao)
        {
            _context = context;
            _sessao = sessao;
        }

        public async Task<Turma> GetAlunosDaTurma(int id)
        {
           return await _context.Turmas.AsNoTracking()
                .Include(x => x.Curso)
                .Include(x => x.TurmaAtas.Where(t => t.TurmaId == id))
                .Include(x => x.Matriculas.Where(m => m.Status == (byte)EMatriculaStatus.Cursando))
                .ThenInclude(m => m.Aluno)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        //GET by id
        public async Task<Turma> GetTurmaById(int id)
        {
            return await _context.Turmas.Include(x => x.Curso).FirstOrDefaultAsync(x => x.Id == id);
        }

        // GET turmas
        public async Task<IEnumerable<Turma>> GetTurmas()
        {
            return await _context.Turmas.AsNoTracking().Include(x => x.Curso).ToListAsync();
        }

        // GET turmas
        public async Task<IEnumerable<Turma>> GetTurmasAtivas()
        {
            return await _context.Turmas.AsNoTracking().Include(x => x.Curso).Where(x => x.Ativo == true).ToListAsync();
        }

        //GET turmas decrescentes id
        public async Task<IEnumerable<Turma>> GetTurmasDesc()
        {
            return await _context.Turmas.AsNoTracking().Include(x => x.Curso).OrderByDescending(x => x.Id).ToListAsync();
        }

        // create turma em uploag bag
        public async Task<Turma> CreateTurma(CreateTurmaViewModel model)
        {
            try
            {
                //verificação se foi feito upload do bag
                string fileName = "";
                if (model.Arquivo != null)
                {
                    fileName = model.Arquivo.FileName;
                    UploadFile(model.Arquivo);
                }
                var turma = new Turma
                {
                    Descricao = model.Descricao.Trim(),
                    Sigla = model.Sigla.Trim(),
                    TurmaSgset = model.TurmaSgset.Trim(),
                    Classroom = model.Classroom,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    Ativo = true,
                    Bag = fileName,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    CursoId = model.CursoId
                };
                _context.Turmas.Add(turma);
                await _context.SaveChangesAsync();

                return turma;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // PUT UpdateTurma
        public async Task<Turma> UpdateTurma(EditTurmaViewModel model)
        {
            try
            {
                //verificação se foi feito upload do bag
                string fileName = "";
                if (model.Arquivo != null)
                {
                    fileName = model.Arquivo.FileName;
                    UploadFile(model.Arquivo);
                }

                Turma turma = await GetTurmaById(model.Id);
                if (turma == null) throw new Exception("Erro! Turma não existe");

                turma.Descricao = model.Descricao.Trim();
                turma.Sigla = model.Sigla.Trim();
                turma.TurmaSgset = model.TurmaSgset.Trim();
                turma.Classroom = model.Classroom;
                turma.DataInicio = model.DataInicio;
                turma.DataFim = model.DataFim;
                turma.Ativo = model.Ativo;
                turma.Bag = (fileName == "" ? turma.Bag : fileName);
                turma.UpdateAt = DateTime.Now;
                turma.CursoId = model.CursoId;

                _context.Turmas.Update(turma);
                await _context.SaveChangesAsync();

                return turma;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Delete turma ou desativa turma
        public async Task DeleteTurma(int id)
        {
            try
            {
                var turma = await GetTurmaById(id);

                if (turma == null) throw new Exception("Erro! curso não existe");
                _context.Turmas.Remove(turma);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                //neste caso se tem uma exceção na hora de gravar na base a turma é desativado
                var turma = await GetTurmaById(id);

                turma.Ativo = false;
                turma.UpdateAt = DateTime.Now;
                _context.Turmas.Update(turma);
                await _context.SaveChangesAsync();
                throw ex;
            }
        }


        // upload do bag para create e update
        private void UploadFile(IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\bags");
            string fileNameWithPath = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        public async Task<Turma> GetTurmaBySgset(string turmaSgset)
        {
            return await _context.Turmas.AsNoTracking().FirstOrDefaultAsync(x => x.TurmaSgset == turmaSgset);
        }

        public async Task CreateAta(CreateTurmaAtaViewModel model)
        {
            try
            {
                Usuario usuario = _sessao.BuscaSessao();
                if (usuario == null)
                {
                    usuario = new Usuario();
                    usuario.Id = 1;
                }

                if (model.Anexo != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Atas");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    FileInfo fileInfo = new FileInfo(model.Anexo.FileName);
                    string extensao = fileInfo.Extension.ToLower();
                    string[] extensoesValidas = new string[] { ".pdf"};

                    if (!extensoesValidas.Contains(extensao))
                    {
                        throw new Exception("0o3 - Ops! erro na extensão do anexo");
                    }


                    string fileName = $@"ATA_{model.TurmaId}_{model.Descricao}_{DateTime.Now.ToString("yyyyMMddHHmm")}{fileInfo.Extension}";
                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Anexo.CopyTo(stream);
                    }

                    var turmaAta = new TurmaAta
                    {
                        UsuarioId = usuario.Id,
                        Descricao = model.Descricao,
                        TurmaId = model.TurmaId,
                        UrlAnexo = @$"./Files/Atas/{fileName}",
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now

                    };

                    _context.TurmaAtas.Add(turmaAta);
                    await _context.SaveChangesAsync();

                    return;   
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<TurmaAta>> GetTurmaAtaByTurmaId(int id)
        {
            return await _context.TurmaAtas.AsNoTracking().Where(x => x.TurmaId == id).ToListAsync();
        }
    }
}
