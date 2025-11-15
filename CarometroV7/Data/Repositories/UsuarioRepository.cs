using CarometroV7.Data.Interfaces;
using CarometroV7.Helper;
using CarometroV7.Models;
using CarometroV7.ViewModel.Usuario;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SecureIdentity.Password;
using System;
using System.Data;

namespace CarometroV7.Data.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly DataContext _context;
        private readonly ISessao _sessao;
        public UsuarioRepository(DataContext context, ISessao sessao)
        {
            _context = context;
            _sessao = sessao;
        }

        //Get usuário para informações de login
        public async Task<Usuario> LoginGeyUserById(int id)
        {
            return await _context.Usuarios.AsNoTracking().Where(x => x.Ativo == true).FirstOrDefaultAsync(u => u.Id == id);
        }

        //Get todos usuários da base
        public async Task<IEnumerable<Usuario>> GetUsuarios() => await _context.Usuarios.AsNoTracking().ToListAsync();

        //Get usuário específico fia id
        public async Task<Usuario> GetUsuarioById(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        //criação de novo usuário na base via ViewModel e aplicações de regras
        public async Task<CreateUsuarioViewModel> CreateUsuario(CreateUsuarioViewModel model)
        {
            var usuarioDb = await GetUsuarioById(model.Id);
            if (usuarioDb != null) throw new Exception("Erro! Usuário já existe");

            var usuario = new Usuario
            {
                Id = model.Id,
                Nome = model.Nome.Trim(),
                Email = model.Email.Trim(),
                Notificar = model.Notificar,
                Perfil = model.Perfil,
                SenhaHash = PasswordHasher.Hash(model.SenhaHash),
                Ativo = true,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();    
            return model;   
        }

        //Editar usuário na base via ViewModel e aplicações de regras
        public async Task<EditUsuarioViewModel> UpdateUsuario(EditUsuarioViewModel model)
        {
            Usuario usuario = await GetUsuarioById(model.Id);

            if (usuario == null) throw new Exception("Erro! Usuário não existe");

            if (string.IsNullOrEmpty(model.SenhaHash))
            {
                model.SenhaHash = usuario.SenhaHash;
            }
            else
            {
                model.SenhaHash = PasswordHasher.Hash(model.SenhaHash);
            }
            usuario.Nome = model.Nome.Trim();
            usuario.SenhaHash = model.SenhaHash;
            usuario.Email = model.Email.Trim();
            usuario.Perfil = (byte)model.Perfil;
            usuario.Notificar = model.Notificar;
            usuario.Ativo = model.Ativo;
            usuario.UpdateAt = DateTime.Now;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return model;
        }

        //apagar usuário da base, caso não seja possível devido a registros, usuário e desativado não consegue logar mais
        public async Task DeleteUsuario(int id)
        {
            try
            {
                Usuario usuario = await GetUsuarioById(id);
                if (usuario == null) throw new Exception("Erro! Usuário não existe");
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                //neste caso se tem uma exceção na hora de gravar na base o usuário é desativado
                Usuario usuario = await GetUsuarioById(id);
                usuario.Ativo = false;
                usuario.Notificar = false;
                usuario.UpdateAt = DateTime.Now;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                throw ex;
            }          
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosNotificados()
        {
            return await _context.Usuarios.AsNoTracking().Where(x => x.Notificar == true).ToListAsync();
        }

        public async Task<bool> AlterarSenha(AlterarSenhaViewModel model)
        {
            Usuario usuarioSessao = _sessao.BuscaSessao();
            if (usuarioSessao == null) return false;

            Usuario usuario = await GetUsuarioById(usuarioSessao.Id);

            if (!PasswordHasher.Verify(usuario.SenhaHash, model.SenhaAtual))
            {
                return false;
            }

            usuario.SenhaHash = model.NovaSenha != null ? PasswordHasher.Hash(model.NovaSenha) : usuario.SenhaHash;
            usuario.UpdateAt = DateTime.Now;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
