using CarometroV6.Data;
using CarometroV6.Enum;
using CarometroV6.Filters;
using CarometroV6.Models;
using CarometroV6.ViewModel.UsuarioViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace CarometroV6.Controllers
{
    [CoordenadorLogado]
    public class UsuarioController : Controller
    {
        public IActionResult Index([FromServices] DataContext context)
        {
            try
            {
                var usuarios = context.Usuarios.AsNoTracking().ToList();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u01 - Ops! não foi possível carregar lista dos usuários Erro: {ex.Message}";
                return View();
            }
        }

        public IActionResult CriarUsuario()
        {
            try
            {
                var model = new CriarUsuarioViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u02 - Ops! não foi possível carregar Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult CriarUsuario(CriarUsuarioViewModel usuarioModel, [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuarioModel);
                }

                var usuario = new Usuario
                {
                    Id = (int)usuarioModel.Login,
                    Nome = usuarioModel.Nome,
                    SenhaHash = PasswordHasher.Hash(usuarioModel.SenhaHash),
                    Email = usuarioModel.Email,
                    Perfil = (byte)usuarioModel.EPerfil,
                    Notificar = usuarioModel.Notificar,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                context.Usuarios.Add(usuario);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Usuário criado com Sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u03 - Ops! não foi possível cadastrar usuário Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult EditarUsuario(int id, [FromServices] DataContext context)
        {
            try
            {
                var usuario = context.Usuarios.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "0u04 - Ops! Usuário não encontrado";
                    return RedirectToAction("Index");
                }

                var editorUsuario = new EditarUsuarioViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Notificar = usuario.Notificar,
                    EPerfil = (EPerfil)usuario.Perfil
                };
                return View(editorUsuario);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "0u05 - Ops! falha no servidor";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EditarUsuario(EditarUsuarioViewModel usuarioModel, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                var editorUsuario = new EditarUsuarioViewModel
                {
                    Id = usuarioModel.Id,
                    Nome = usuarioModel.Nome,
                    Email = usuarioModel.Email,
                    Notificar = usuarioModel.Notificar,
                    EPerfil = usuarioModel.EPerfil
                };
                return View(editorUsuario);
            }

            try
            {
                var usuario = context.Usuarios.FirstOrDefault(x => x.Id == usuarioModel.Id);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "0u06 - Ops! Usuário não encontrado";
                    return RedirectToAction("Index");
                }

                usuario.Perfil = (byte)usuarioModel.EPerfil;
                usuario.Nome = usuarioModel.Nome;
                usuario.SenhaHash = (usuarioModel.SenhaHash != null ? PasswordHasher.Hash(usuarioModel.SenhaHash) : usuario.SenhaHash);
                usuario.Email = usuarioModel.Email;
                usuario.Notificar = usuarioModel.Notificar;
                usuario.UpdateAt = DateTime.Now;

                context.Usuarios.Update(usuario);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Usuário atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u07 - Ops! Erro Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RemoverUsuario(int id, [FromServices] DataContext context)
        {
            try
            {
                var usuario = context.Usuarios.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "0u08* - Ops! Usuário não encontrado";
                    return RedirectToAction("Index");
                }
                return View(usuario);

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u09 - Ops! Erro no Servidor. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }

        }

        public IActionResult ExcluirUsuario(int id, [FromServices] DataContext context)
        {
            try
            {
                var usuario = context.Usuarios.FirstOrDefault(x => x.Id == id);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "0u10 - Ops! Usuário não encontrado";
                    return RedirectToAction("Index");
                }

                context.Usuarios.Remove(usuario);
                context.SaveChanges();

                TempData["MensagemSucesso"] = "Usuário excluido com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u11 - Ops! não foi excluir usuário. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
