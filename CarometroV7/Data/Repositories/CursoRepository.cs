using CarometroV7.Data.Interfaces;
using CarometroV7.Models;
using CarometroV7.ViewModel.Curso;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarometroV7.Data.Repositories
{

    public class CursoRepository : ICurso
    {
        private readonly DataContext _context;
        public CursoRepository(DataContext context)
        {
            _context = context;
        }

        //Get curso by id
        public async Task<Curso> GetCursoById(int id)
        {
            return await _context.Cursos.FirstOrDefaultAsync(x => x.Id == id);
        }

        // Get todos os cursos
        public async Task<IEnumerable<Curso>> GetCursos() => await _context.Cursos.AsNoTracking().ToListAsync();

        // Create curso com aplicação da ViewModel
        public async Task<CreateCursoViewModel> CreateCurso(CreateCursoViewModel model)
        {
            var curso = new Curso
            {
                Descricao = model.Descricao.Trim(),
                Tipo = model.Tipo,
                Cor = model.Cor,
                Ativo = true,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return model;
        }


        //Update curso com uso do ViewModel
        public async Task<EditCursoViewModel> UpdateCurso(EditCursoViewModel model)
        {
            Curso curso = await GetCursoById(model.Id);
           
            if (curso == null) throw new Exception("Erro! Curso não existe");

            curso.Descricao = model.Descricao.Trim();
            curso.Tipo = model.Tipo;
            curso.Cor = model.Cor;
            curso.Ativo = model.Ativo;
            curso.UpdateAt = DateTime.Now;

            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
            return model;
        }

        // Delete curso by id
        public async Task DeleteCurso(int id)
        {
            try
            {
                var curso = await GetCursoById(id);

                if (curso == null) throw new Exception("Erro! curso não existe");
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                //neste caso se tem uma exceção na hora de gravar na base o curso é desativado
                var curso = await GetCursoById(id);
                curso.Ativo = false;
                curso.UpdateAt = DateTime.Now;
                _context.Cursos.Update(curso);
                await _context.SaveChangesAsync();
                throw ex;
            }
        }

        public async Task<IEnumerable<Curso>> GetCursosAtivos()
        {
            return await _context.Cursos.AsNoTracking().Where(x => x.Ativo == true).Include(x => x.Turmas.Where(x => x.Ativo == true)).ToListAsync();
        }

    }
}
