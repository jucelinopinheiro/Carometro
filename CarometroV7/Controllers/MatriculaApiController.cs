using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.Models;
using CarometroV7.ViewModel;
using CarometroV7.ViewModel.Matricula;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class MatriculaApiController : ControllerBase
    {
        private readonly IMatricula _matriculaRepository;

        public MatriculaApiController(IMatricula matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        //grana do bando uma nova movimentação da apudação e atualiza do local do patrimônio
        [HttpPost("v1/matriculas/enturmar")]
        public async Task<IActionResult> PostAsync([FromBody] CreateMatriculaViewModel model)
        {

            try
            {
                var novaMatricula = new Matricula();
                var matricula = await _matriculaRepository.GetMatriculaByIdSgset(model.MatriculaSgset);
                if (matricula == null)
                {   
                     novaMatricula = await _matriculaRepository.CreateMatricula(model);
                }

                return Created($"v1/turmas/enturmar/{novaMatricula.Id}", new ResultViewModel<Matricula>(matricula));

            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("0at2 - Matricula Cadastrada"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("0at3 - Erro interno no servidor!"));
            }
        }
    }
}
