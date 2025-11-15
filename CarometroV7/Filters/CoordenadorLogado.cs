using CarometroV7.Enum;
using CarometroV7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CarometroV7.Filters
{
    public class CoordenadorLogado : ActionFilterAttribute
    {
        //Sobrescrevando a classe que carraga os controladores
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //busca sessão do usuário
            var sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuario");
            //verifica se existe sessão
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                //se for nulo ou vazio
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Access" }, { "action", "Login" } });
            }
            else
            {
                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(sessaoUsuario);
                if (usuario == null)
                {
                    //caso tenha algum erro na sessão redireciona para access
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Access" }, { "action", "Login" } });
                }else if (usuario.Perfil != (byte)EPerfil.Coordenador)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Access" }, { "action", "Login" } });
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
