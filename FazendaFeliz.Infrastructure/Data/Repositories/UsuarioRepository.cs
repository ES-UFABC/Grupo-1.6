﻿using Microsoft.EntityFrameworkCore;
using FazendaFeliz.ApplicationCore.Business;
using FazendaFeliz.ApplicationCore.Interfaces.Repository;
using FazendaFeliz.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FazendaFeliz.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public Usuario ObterPorEmail(string email)
        {
            return _Context.Usuarios.Where(u => u.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public void AdicionarCasoInexistente(Usuario usuario)
        {
            var usuarioEncontrado = _Context.Usuarios.Where(u => u.Email == usuario.Email)
                                                            .Select(u => u.Id)
                                                            .FirstOrDefault();
            if (usuarioEncontrado == 0)
            {
                _Context.Usuarios.Add(usuario);
                _Context.SaveChanges();
            }
        }

        public async Task<Usuario> ObterUsuarioOuInserir(string email, string nome)
        {

            Usuario usuario = await _Context.Usuarios.Where(u => u.Email == email).FirstOrDefaultAsync();
            
            if (usuario == null)
            {
                usuario.Email = email;
                usuario.Nome = nome;

                await Adicionar(usuario);
            }

            return usuario;
        }

        public async Task<int> ObterIdOuInserir(string email, string nome)
        {
            Usuario usuario = new Usuario()
            {
                Id = _Context.Usuarios.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault()
            };
            if (usuario.Id == 0)
            {
                usuario.Email = email;
                usuario.Nome = nome;

                await Adicionar(usuario);
            }

            return usuario.Id;
        }

        public async Task<int> ObterIdPorEmail(string email)
        {
            int id = await _Context.Usuarios.Where(u => u.Email == email).AsNoTracking().Select(u => u.Id).FirstOrDefaultAsync();

            return id;
        }
    }
}
