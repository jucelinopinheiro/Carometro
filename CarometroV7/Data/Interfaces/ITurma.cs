using CarometroV7.Models;
using CarometroV7.ViewModel.Turma;

namespace CarometroV7.Data.Interfaces
{
    public interface ITurma
    {
        Task<Turma> GetAlunosDaTurma(int id);
        Task<IEnumerable<Turma>> GetTurmas();
        Task<IEnumerable<Turma>> GetTurmasAtivas();
        Task<IEnumerable<Turma>> GetTurmasDesc();
        Task<Turma> GetTurmaById(int id);
        Task<Turma> GetTurmaBySgset(string turmaSgset);
        Task<Turma> CreateTurma(CreateTurmaViewModel model);
        Task<Turma> UpdateTurma(EditTurmaViewModel model);
        Task DeleteTurma(int id);
        Task CreateAta(CreateTurmaAtaViewModel model);
        Task<IEnumerable<TurmaAta>> GetTurmaAtaByTurmaId (int id);

    }
}
