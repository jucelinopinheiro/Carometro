using CarometroV7.Data.Interfaces;
using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarometroV7.Data.Repositories
{
    public class AnexoOcorrenciaRepository : IAnexoOcorrencia
    {
        private readonly DataContext _dataContext;

        public AnexoOcorrenciaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateAnexoOcorrencia(AnexoOcorrencia model)
        {
            _dataContext.Add(model);
            await _dataContext.SaveChangesAsync();
            return;
        }

        public Task DeleteAnexoOcorrencia(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetAnexoOcorrencia()
        {
            throw new NotImplementedException();
        }

        public async Task<AnexoOcorrencia> GetAnexoOcorrenciaById(int id) => await _dataContext.AnexoOcorrencias.FirstAsync(x => x.Id == id);

        public async Task UpdateAnexoOcorrencia(AnexoOcorrencia model)
        {
            try
            {
                AnexoOcorrencia anexo = await GetAnexoOcorrenciaById(model.Id);
                if (anexo != null) throw new Exception("Erro anexo ocorrência");

                anexo.Descricao = model.Descricao;
                anexo.UrlAnexo = model.UrlAnexo;
                anexo.UpdateAt = DateTime.Now;
                _dataContext.Update(anexo);
                await _dataContext.SaveChangesAsync();
                return;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


    }
}
