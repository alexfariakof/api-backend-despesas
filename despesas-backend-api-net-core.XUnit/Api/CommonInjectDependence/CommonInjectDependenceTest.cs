﻿using DataSeeders;
using despesas_backend_api_net_core.CommonInjectDependence;
using Domain.Core;
using Domain.Core.Interface;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Api.CommonInjectDependence;
#pragma warning disable xUnit2002 // Do not use null check on value type
public class CommonInjectDependenceTest
{
    [Fact]
    public void AddServices_Should_Register_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddServices();

        // Assert

        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<DespesaVM>) && descriptor.ImplementationType == typeof(DespesaBusinessImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<ReceitaVM>) && descriptor.ImplementationType == typeof(ReceitaBusinessImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<CategoriaVM>) && descriptor.ImplementationType == typeof(CategoriaBusinessImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IControleAcessoBusiness) && descriptor.ImplementationType == typeof(ControleAcessoBusinessImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(ILancamentoBusiness) && descriptor.ImplementationType == typeof(LancamentoBusinessImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IUsuarioBusiness) && descriptor.ImplementationType == typeof(UsuarioBusinessImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IImagemPerfilUsuarioBusiness) && descriptor.ImplementationType == typeof(ImagemPerfilUsuarioBusinessImpl)));
    }

    [Fact]
    public void AddRepositories_Should_Register_Repositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRepositories();

        // Assert
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<DespesaVM>) && descriptor.ImplementationType == typeof(GenericRepositorio<DespesaVM>)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<ReceitaVM>) && descriptor.ImplementationType == typeof(GenericRepositorio<ReceitaVM>)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<CategoriaVM>) && descriptor.ImplementationType == typeof(GenericRepositorio<CategoriaVM>)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<Usuario>) && descriptor.ImplementationType == typeof(UsuarioRepositorioImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IControleAcessoRepositorio) && descriptor.ImplementationType == typeof(ControleAcessoRepositorioImpl)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IEmailSender) && descriptor.ImplementationType == typeof(EmailSender)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(ILancamentoRepositorio) && descriptor.ImplementationType == typeof(LancamentoRepositorioImpl)));
    }

    [Fact]
    public void CreateDataBaseInMemory_Should_Add_Context_And_DataSeeder()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.CreateDataBaseInMemory();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetService<RegisterContext>();
        var dataSeeder = serviceProvider.GetService<IDataSeeder>();

        Assert.NotNull(context);
        Assert.NotNull(dataSeeder);
    }
}
#pragma warning restore xUnit2002 // Do not use null check on value type