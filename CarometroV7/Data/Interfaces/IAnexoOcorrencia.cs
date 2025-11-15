using CarometroV7.Models;

namespace CarometroV7.Data.Interfaces
{
    public interface IAnexoOcorrencia
    {
        Task GetAnexoOcorrencia();
        Task<AnexoOcorrencia> GetAnexoOcorrenciaById(int id);
        Task CreateAnexoOcorrencia(AnexoOcorrencia model);
        Task UpdateAnexoOcorrencia(AnexoOcorrencia model);
        Task DeleteAnexoOcorrencia(int id);
    }
}
