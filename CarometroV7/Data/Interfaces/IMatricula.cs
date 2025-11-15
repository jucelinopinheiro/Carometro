using CarometroV7.Models;
using CarometroV7.ViewModel.Matricula;

namespace CarometroV7.Data.Interfaces
{
    public interface IMatricula
    {
        Task<Matricula> GetMatriculaAluno(int id);
        Task<IEnumerable<Matricula>> GetAllMatriculas();
        Task<IEnumerable<Matricula>> GetMatriculasAtivas();
        Task<IEnumerable<Matricula>> GetMatriculasConcluidas();
        Task<IEnumerable<Matricula>> GetMatriculasEvadidas();
        Task<Matricula> GetMatriculaById(int id);
        Task<Matricula> GetMatriculaByIdSgset(int id);
        Task<Matricula> CreateMatricula(CreateMatriculaViewModel model);
        Task<Matricula> UpdateMatricula(Matricula model);
        Task DeleteMatricula(int id);
    }
}