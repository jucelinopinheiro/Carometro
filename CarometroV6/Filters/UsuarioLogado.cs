﻿using CarometroV6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CarometroV6.Filters
{
    //herdendo da classe ActionFilterAttribute
    public class UsuarioLogado : ActionFilterAttribute
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
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "login" }, { "action", "index" } });
            }
            else
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(sessaoUsuario);
                if (usuario == null)
                {
                    //caso tenha algum erro na sessão redireciona para login
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "login" }, { "action", "index" } });
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
