using CarometroV7.Data.Interfaces;
using CarometroV7.Models;
using CarometroV7.ViewModel.Aluno;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarometroV7.Data.Repositories
{
    public class AlunoRepository : IAluno
    {
        private readonly DataContext _context;

        public AlunoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Aluno> GetAlunoById(int id)
        {
            return await _context.Alunos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Aluno> GetAlunoByCpf(string cpf)
        {
            return await _context.Alunos.FirstOrDefaultAsync(x => x.Cpf == cpf);
        }

        public async Task<IEnumerable<Aluno>> GetAlunosAtivos()
        {
            return await _context.Alunos.AsNoTracking().Where(x => x.Ativo == true).OrderBy(x => x.Nome).ToListAsync();
        }

        // Get all alunos ordey by name
        public async Task<IEnumerable<Aluno>> GetAllAlunos()
        {
            return await _context.Alunos.AsNoTracking().OrderBy(x => x.Nome).ToListAsync();
        }

        public async Task<Aluno> CreateAluno(CreateAlunoViewModel model)
        {
            try
            {
                //verificação se foi feito upload do bag
                string fileName = "";
                if (model.Arquivo != null)
                {
                    fileName = model.Cpf.Replace(".", "").Replace("-", "") + ".png";
                    UploadFile(model.Arquivo, fileName);
                }

                var aluno = new Aluno
                {
                    Nome = model.Nome.Trim(),
                    CelAluno = model.CelAluno.Trim(),
                    EmailAluno = model.EmailAluno.Trim(),
                    Nascimento = model.Nascimento,
                    Rg = model.Rg,
                    Cpf = model.Cpf,
                    Pai = model.Pai.Trim(),
                    CelPai = model.CelPai.Trim(),
                    Mae = model.Mae.Trim(),
                    CelMae = model.CelMae.Trim(),
                    Foto = model.Cpf.Replace(".", "").Replace("-", "") + ".png",
                    Pne = model.Pne,
                    ObsAluno = model.ObsAluno,
                    Ativo = true,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();
                return aluno;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<Aluno> UpdateAluno(EditAlunoViewModel model)
        {
            try
            {
                Aluno aluno = await GetAlunoById(model.Id);
                if (aluno == null) throw new Exception("Erro! Aluno não existe");

                //verificação se foi feito upload do bag
                string fileName = "";
                if (model.Arquivo != null)
                {
                    fileName = model.Cpf.Replace(".", "").Replace("-", "") + ".png";
                    UploadFile(model.Arquivo, fileName);
                }

                aluno.Nome = model.Nome.Trim();
                aluno.CelAluno = model.CelAluno.Trim();
                aluno.EmailAluno = model.EmailAluno.Trim();
                aluno.Nascimento = model.Nascimento;
                aluno.Rg = model.Rg;
                aluno.Cpf = model.Cpf;
                aluno.Pai = model.Pai.Trim();
                aluno.CelPai = model.CelPai.Trim();
                aluno.Mae = model.Mae.Trim();
                aluno.CelMae = model.CelMae.Trim();
                aluno.Pne = model.Pne;
                aluno.ObsAluno = model.ObsAluno;
                aluno.Ativo = model.Ativo;
                aluno.UpdateAt = DateTime.Now;

                
                _context.Alunos.Update(aluno);
                await _context.SaveChangesAsync();
                return aluno;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAluno(int id)
        {
            try
            {
                var aluno = await GetAlunoById(id);

                if (aluno == null) throw new Exception("Erro! aluno não existe");
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                //neste caso se tem uma exceção na hora de gravar na base o aluno é desativado
                var aluno = await GetAlunoById(id);

                aluno.Ativo = false;
                aluno.UpdateAt = DateTime.Now;
                _context.Alunos.Update(aluno);
                await _context.SaveChangesAsync();
                throw ex;
            }
        }

        // upload do bag para create e update
        private void UploadFile(IFormFile file, string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files\\fotos");
            string fileNameWithPath = Path.Combine(path, filename);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        public async Task<IEnumerable<Aluno>> GetAlunoByName(string name)
        {
            return await _context.Alunos.AsNoTracking().Where(x => x.Nome.Contains(name)).OrderBy(x => x.Nome).Include(x => x.Matriculas).ThenInclude(m => m.Turma).ToListAsync();
        }
    }
}