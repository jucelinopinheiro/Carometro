using CarometroV7.Models;
using CarometroV7.ViewModel.Aluno;

namespace CarometroV7.Data.Interfaces
{
    public interface IAluno
    {
        Task<IEnumerable<Aluno>> GetAllAlunos();
        Task<IEnumerable<Aluno>> GetAlunosAtivos();
        Task<Aluno> GetAlunoById(int id);
        Task<Aluno> GetAlunoByCpf(string cpf);
        Task<IEnumerable<Aluno>> GetAlunoByName(string name);
        Task<Aluno> CreateAluno(CreateAlunoViewModel model);
        Task<Aluno> UpdateAluno(EditAlunoViewModel model);
        Task DeleteAluno(int id);

    }
}
