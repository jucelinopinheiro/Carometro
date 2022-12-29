using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel;
using CarometroV6.ViewModel.TurmaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CarometroV6.Controllers
{

    [UsuarioLogado]
    public class TurmaApiController : Controller
    {
        [HttpGet("v1/turmas/{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id, [FromServices] DataContext context)
        {
            try
            {
                var turmas = await context.Turmas.Where(x => x.CursoId == id).ToListAsync();

                return Ok(new ResultViewModel<List<Turma>>(turmas));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Turma>("0at1 - Falha interna no Servidor"));
            }
        }

        
    }
}
