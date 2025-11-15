using CarometroV7.Models;
using CarometroV7.ViewModel.Curso;

namespace CarometroV7.Data.Interfaces
{
    public interface ICurso
    {
        Task<IEnumerable<Curso>> GetCursos();
        Task<IEnumerable<Curso>> GetCursosAtivos();
        Task<Curso> GetCursoById(int id);
        Task<CreateCursoViewModel> CreateCurso(CreateCursoViewModel model);
        Task<EditCursoViewModel> UpdateCurso(EditCursoViewModel model);
        Task DeleteCurso(int id);

    }
}
