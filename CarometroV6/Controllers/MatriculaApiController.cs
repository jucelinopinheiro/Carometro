using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel;
using CarometroV6.ViewModel.TurmaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarometroV6.Controllers
{
    [UsuarioLogado]
    public class MatriculaApiController : Controller
    {
        //grana do bando uma nova movimentação da apudação e atualiza do local do patrimônio
        [HttpPost("v1/matriculas/enturmar")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EnturmarViewModel model,
            [FromServices] DataContext context)
        {

            try
            {
                var matricula = context.Matriculas.AsNoTracking().FirstOrDefault(x => x.MatriculaSgset == model.MatriculaSgset);
                if (matricula == null)
                {
                    matricula = new Matricula
                    {
                        MatriculaSgset = model.MatriculaSgset,
                        AlunoId = model.AlunoId,
                        TurmaId = model.TurmaId,
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now
                    };
                    await context.Matriculas.AddAsync(matricula);
                    await context.SaveChangesAsync();
                }

                return Created($"v1/turmas/enturmar/{matricula.Id}", new ResultViewModel<Matricula>(matricula));

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
