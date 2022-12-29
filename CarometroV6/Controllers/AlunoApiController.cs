using CarometroV6.Data;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarometroV6.Controllers
{
    [ApiController]
    [UsuarioLogado]
    public class AlunoApiController : Controller
    {
        [HttpGet("v1/alunos/busca")]
        public async Task<IActionResult> GetAsync(string cpf, [FromServices] DataContext context)
        {
            try
            {
                var aluno = await context.Alunos.AsNoTracking().FirstOrDefaultAsync(x => x.Cpf == cpf);
                if (aluno == null)
                    return NotFound(new ResultViewModel<Aluno>("Aluno não cadastrado"));
                return Ok(new ResultViewModel<Aluno>(aluno));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Aluno>("0aa1 - Falha interna no Servidor"));
            }
        }
    }
}
