using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.Models;
using CarometroV7.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CarometroV7.Controllers
{

    [ApiController]
    [UsuarioLogado]
    public class AlunoApiController : ControllerBase
    {
        private readonly IAluno _alunoRepository;

        public AlunoApiController(IAluno alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        [HttpGet("v1/alunos/busca")]
        public async Task<IActionResult> GetAsync(string cpf)
        {
            try
            {
                var aluno = await _alunoRepository.GetAlunoByCpf(cpf);
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
