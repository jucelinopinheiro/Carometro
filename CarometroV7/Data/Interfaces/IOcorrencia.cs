using CarometroV7.Models;
using CarometroV7.ViewModel.Ocorrencia;

namespace CarometroV7.Data.Interfaces
{
    public interface IOcorrencia
    {
        Task<IEnumerable<Ocorrencia>> GetAllOcorrencias();
        Task<IEnumerable<Ocorrencia>> GetAllOcorrenciasAlunoId(int id);
        Task<Ocorrencia> GetOcorrenciaById(int id);
        Task<Ocorrencia> CreatOcorrencia(CriarOcorrenciaViewModel model);
        Task<Ocorrencia> EditOcorrencia(EditarOcorrenciaViewModel model);
        Task DeleteOcorrencia(int id);
        Task<bool> Anexar(CriarAnexosViewModel model);
    }
}