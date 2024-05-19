﻿using Domain.Entities.ValueObjects;
using Fakers.v2;
using Repository.Persistency.Abstractions;

namespace Repository.Persistency.Implementations.Fixtures;
public sealed class ControleAcessoRepositorioFixture : IDisposable
{
    public RegisterContext Context { get; private set; }
    public Mock<ControleAcessoRepositorioImpl> Repository { get; set; }
    public Mock<IControleAcessoRepositorioImpl> MockRepository { get; private set; }

    public ControleAcessoRepositorioFixture()
    {
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "ControleAcessoRepositorioImpl").Options;
        Context = new RegisterContext(options);
        Context.PerfilUsuario.Add(new PerfilUsuario(PerfilUsuario.PerfilType.Administrador));
        Context.PerfilUsuario.Add(new PerfilUsuario(PerfilUsuario.PerfilType.Usuario));
        Context.SaveChanges();

        var lstControleAcesso = ControleAcessoFaker.Instance.ControleAcessos(5);
        lstControleAcesso.ForEach(c => c.Usuario.PerfilUsuario = Context.PerfilUsuario.First(tc => tc.Id == c.Usuario.PerfilUsuario.Id));
        Context.AddRange(lstControleAcesso);
        Context.SaveChanges();

        Repository = new Mock<ControleAcessoRepositorioImpl>(Context);
        MockRepository = Mock.Get<IControleAcessoRepositorioImpl>(Repository.Object);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}