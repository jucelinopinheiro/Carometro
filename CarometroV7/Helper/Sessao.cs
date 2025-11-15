using CarometroV7.Models;
using Newtonsoft.Json;

namespace CarometroV7.Helper
{
    public class Sessao: ISessao
    {
        private readonly IHttpContextAccessor _httpContext;
        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        //cria sessão do usuário
        public void CriarSessao(Usuario userModel)
        {
            //prepaara para receber um model e transformar em string no formato json
            string usuario = JsonConvert.SerializeObject(userModel);

            //criação da sessão com usuario no formato json
            _httpContext.HttpContext.Session.SetString("sessaoUsuario", usuario);
        }

        //busca sessão do usuário 
        public Usuario BuscaSessao()
        {
            //buscas sessão na chave informada
            var sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoUsuario");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<Usuario>(sessaoUsuario);
        }

        //destruindo sessão do usuário
        public void RemoverSessao()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuario");
        }
    }
}
