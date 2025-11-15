using CarometroV7.access;
using CarometroV7.Data.Interfaces;
using CarometroV7.Enum;
using CarometroV7.Filters;
using CarometroV7.Models;
using CarometroV7.ViewModel.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace CarometroV7.Controllers
{
    [UsuarioLogado]
    public class UsuarioController : Controller
    {
        private readonly IUsuario _usuarioRepository;

        public UsuarioController(IUsuario usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        //Get usuários
        [AdministradorLogado]
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioRepository.GetUsuarios();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro! {ex.Message}";
                return View();
            }
            
        }


        // GET: Usuarios/Create
        [AdministradorLogado]
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> items = EPerfil.GetValues(typeof(EPerfil))
                                          .Cast<EPerfil>()
                                          .Select(e => new SelectListItem
                                          {
                                              Text = e.ToString(),
                                              Value = ((int)e).ToString()
                                          })
                                          .ToList();
            ViewBag.Perfil = items;
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [AdministradorLogado]
        public async Task<IActionResult>Create(CreateUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _usuarioRepository.CreateUsuario(model);
                TempData["MensagemSucesso"] = "Usuário criado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível cadastrar usuário Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Usuarios/Edit/5
        [AdministradorLogado]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Usuario usuario = await  _usuarioRepository.GetUsuarioById(id);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "0u04 - Ops! Usuário não encontrado";
                    return RedirectToAction("Index");
                }
                EditUsuarioViewModel model = new EditUsuarioViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    SenhaHash = "",
                    Perfil = (EPerfil)usuario.Perfil,
                    Notificar = usuario.Notificar,
                    Ativo = usuario.Ativo
                };
                List<SelectListItem> items = EPerfil.GetValues(typeof(EPerfil))
                                          .Cast<EPerfil>()
                                          .Select(e => new SelectListItem
                                          {
                                              Text = e.ToString(),
                                              Value = ((int)e).ToString()
                                          })
                                          .ToList();
                ViewBag.Perfil = items;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível editar o usuário Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Usuarios/Edit/
        [HttpPost]
        [AdministradorLogado]
        public async Task<IActionResult> Edit(EditUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _usuarioRepository.UpdateUsuario(model);

                TempData["MensagemSucesso"] = "Usuário atualizado com Sucesso!";
                return RedirectToAction("Index");


            }
            catch (Exception ex) 
            {
                TempData["MensagemErro"] = $"Ops! não foi possível atualizar o usuário Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try 
            {
                var usuario = await _usuarioRepository.GetUsuarioById(id);
               
                if(usuario == null)
                {
                    TempData["MensagemErro"] = $"Ops! erro ao carregar o usuário";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch(Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível localizar o usuário Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Usuarios/Delete/5
        [AdministradorLogado]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetUsuarioById(id);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = $"Ops! Usuário não localizado";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! Falha no servidor Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [AdministradorLogado]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _usuarioRepository.DeleteUsuario(id);
                TempData["MensagemSucesso"] = "Usuário excluido com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Não foi possível excluir usuário, usuário desativado! {ex.Message}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops! não foi possível excluir o usuário. Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [UsuarioLogado]
        public IActionResult AlterarSenha()
        {
            return View();
        }

        [HttpPost]
        [UsuarioLogado]
        public async Task<IActionResult>AlterarSenha(AlterarSenhaViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Ops! Erro no envio da nova senha.";
                return View();
            }
            try
            {
                bool resp = await _usuarioRepository.AlterarSenha(modelo);
                
                if(!resp)
                {
                    TempData["MensagemErro"] = "Ops! Erro no envio da nova senha.";
                    return View();
                }

                TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                return View();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"0u14 - Ops! Erro no Servidor. Erro: {ex.Message}";
                return View();
            }
        }
    }
}
