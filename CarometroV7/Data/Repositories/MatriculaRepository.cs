using CarometroV7.Data.Interfaces;
using CarometroV7.Enum;
using CarometroV7.Models;
using CarometroV7.ViewModel.Matricula;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Data.Repositories
{
    public class MatriculaRepository : IMatricula
    {
        private readonly DataContext _context;

        public MatriculaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Matricula> GetMatriculaAluno(int id)
        {
            var matricula = await _context.Matriculas.AsNoTracking()
                                                        .Include(x => x.Aluno)
                                                            .ThenInclude(x => x.Ocorrencias)
                                                                .ThenInclude(o => o.Turma)
                                                         .Include(x => x.Aluno)
                                                            .ThenInclude(x => x.Ocorrencias)
                                                                .ThenInclude(o => o.Usuario)
                                                         .Include(x => x.Aluno)
                                                            .ThenInclude(x => x.Ocorrencias)
                                                                .ThenInclude(o => o.Anexos)
                                                        .Include(x => x.Turma)
                                                            .ThenInclude(t => t.Curso)
                                                        .FirstOrDefaultAsync(x => x.Id == id);

            if (matricula != null && matricula.Aluno?.Ocorrencias != null)
            {
                matricula.Aluno.Ocorrencias = matricula.Aluno.Ocorrencias
                    .OrderByDescending(o => o.Id)
                    .ToList();
            }

            return matricula;
        }
        public async Task<IEnumerable<Matricula>> GetAllMatriculas()
        {
            return await _context.Matriculas.AsNoTracking().Include(x => x.Aluno).Include(x => x.Turma).ToListAsync();
        }
        public async Task<IEnumerable<Matricula>> GetMatriculasAtivas()
        {
            return await _context.Matriculas.AsNoTracking().Where(x => x.Status == 1 ).Include(x => x.Aluno).Include(x => x.Turma).ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasConcluidas()
        {
            return await _context.Matriculas.AsNoTracking().Where(x => x.Status == 2).Include(x => x.Aluno).Include(x => x.Turma).ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasEvadidas()
        {
            return await _context.Matriculas.AsNoTracking().Where(x => x.Status == 3).Include(x => x.Aluno).Include(x => x.Turma).ToListAsync();
        }
        public async Task<Matricula> GetMatriculaById(int id)
        {
            return await _context.Matriculas.Include(x => x.Aluno).Include(x => x.Turma).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Matricula> GetMatriculaByIdSgset(int id)
        {
            return await _context.Matriculas.Include(x => x.Aluno).Include(x => x.Turma).FirstOrDefaultAsync(x => x.MatriculaSgset == id);
        }
        public async Task<Matricula> CreateMatricula(CreateMatriculaViewModel model)
        {
            try
            {
                var matricula = new Matricula
                {
                    MatriculaSgset = model.MatriculaSgset,
                    DataMatricula = model.DataMatricula,
                    AlunoId = model.AlunoId,
                    TurmaId = model.TurmaId,
                    Status =  (byte)EMatriculaStatus.Cursando,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };
                _context.Matriculas.Add(matricula);
                await _context.SaveChangesAsync();

                return matricula;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Matricula> UpdateMatricula(Matricula model)
        {
            try
            {
                var matricula = await GetMatriculaById(model.Id);
                
                if (matricula == null) throw new Exception("Erro! matrícula não existe");

                matricula.MatriculaSgset = model.MatriculaSgset;
                matricula.DataMatricula = model.DataMatricula;
                matricula.TurmaId = model.TurmaId;
                matricula.Status = model.Status;
                matricula.UpdateAt = DateTime.Now;

                _context.Matriculas.Update(matricula);
                await _context.SaveChangesAsync();

                return matricula;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteMatricula(int id)
        {
            try
            {
                var matricula = await GetMatriculaById(id);
                if (matricula == null) throw new Exception("Erro! matrícula não existe");

                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                //neste caso se tem uma exceção na hora de gravar na base a matricula é desativado
                var matricula = await GetMatriculaById(id);

                matricula.Status = 3;
                matricula.UpdateAt = DateTime.Now;
                _context.Matriculas.Update(matricula);
                await _context.SaveChangesAsync();
                throw ex;
            }
        }
    }
}
