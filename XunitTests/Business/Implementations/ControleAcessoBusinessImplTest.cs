﻿using Business.Authentication;
using Business.Authentication.Interfaces;
using Business.Dtos.v2;
using Domain.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Persistency.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using Fakers.v2;
using Business.Dtos.Core;
using System.Linq.Expressions;
using AutoMapper;
using Business.Dtos.Core.Profile;

namespace Business;
public class ControleAcessoBusinessImplTest
{
    private readonly Mock<IControleAcessoRepositorioImpl> _repositorioMock;
    private readonly ControleAcessoBusinessImpl<ControleAcessoDto, LoginDto> _controleAcessoBusiness;
    private readonly Mock<ITokenConfiguration> _tokenConfigurationMock;
    private Mapper _mapper;

    public ControleAcessoBusinessImplTest()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
        var signingConfigurations = new SigningConfigurations();
        configuration.GetSection("TokenConfigurations").Bind(signingConfigurations);
        var tokenConfigurations = new TokenConfiguration();
        configuration.GetSection("TokenConfigurations").Bind(tokenConfigurations);
        _tokenConfigurationMock = new Mock<ITokenConfiguration>();
        _repositorioMock = new Mock<IControleAcessoRepositorioImpl>();
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.AddProfile<ControleAcessoProfile>(); }));
        _controleAcessoBusiness = new ControleAcessoBusinessImpl<ControleAcessoDto, LoginDto>(_mapper, _repositorioMock.Object, signingConfigurations, tokenConfigurations, new EmailSender());
    }

    [Fact]
    public void Create_Should_ControleAcesso_Returns_True()
    {
        // Arrange
        var controleAcesso = ControleAcessoFaker.Instance.GetNewFakerVM();

        _repositorioMock.Setup(repo => repo.Create(It.IsAny<ControleAcesso>()));

        // Act
        _controleAcessoBusiness.Create(controleAcesso);

        // Assert        
        _repositorioMock.Verify(repo => repo.Create(It.IsAny<ControleAcesso>()), Times.Once);
    }

    [Fact]
    public void ValidateCredentials_Should_Return_Valid_Credentials_And_AccessToken()
    {
        // Arrange
        var controleAcesso = new LoginDto { Email = "teste@teste.com", Senha = "teste", };

        var usuario = new Usuario
        {
            Id = 1,
            Email = "teste@teste.com",
            StatusUsuario = StatusUsuario.Ativo
        };

        _repositorioMock.Setup(repo => repo.IsValidPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _repositorioMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<ControleAcesso, bool>>>())).Returns(new ControleAcesso { Login = controleAcesso.Email, Usuario = usuario });

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials(controleAcesso);

        // Assert
        Assert.True(result.Authenticated);
        Assert.NotNull(result.AccessToken);
    }

    [Fact]
    public void ValidateCredentials_Should_Returns_Usaurio_Inexistente()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "teste@teste.com" };
        _repositorioMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<ControleAcesso, bool>>>())).Returns<ControleAcesso>(null);

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials(loginDto);

        // Assert
        Assert.False(result.Authenticated);
        Assert.Contains("Usuário inexistente!", result.Message);
    }

    [Fact]
    public void ValidateCredentials_Should_Returns_Usuario_Inativo()
    {
        // Arrange
        var controleAcesso = ControleAcessoFaker.Instance.GetNewFaker();
        var usuarioInativo = UsuarioFaker.Instance.GetNewFaker();
        usuarioInativo.StatusUsuario = StatusUsuario.Inativo;
        controleAcesso.Usuario = usuarioInativo;
        _repositorioMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<ControleAcesso, bool>>>())).Returns(controleAcesso);

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials(new LoginDto { Email = controleAcesso.Login, Senha = controleAcesso.Senha });

        // Assert
        Assert.False(result.Authenticated);
        Assert.Contains("Usuário Inativo!", result.Message);
    }

    [Fact]
    public void ValidateCredentials_Should_Returns_Email_Inexistente()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "teste@teste.com", Senha = "teste", };
        var usuario = new Usuario
        {
            Id = 1,
            Email = "teste@teste.com",
            StatusUsuario = StatusUsuario.Ativo
        };

        _repositorioMock.Setup(repo => repo.IsValidPassword(loginDto.Email, loginDto.Senha)).Returns(true);
        _repositorioMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<ControleAcesso, bool>>>())).Returns<ControleAcesso>(null);

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials(loginDto);

        // Assert
        Assert.False(result.Authenticated);
        Assert.Contains("Usuário inexistente!", result.Message);
    }

    [Fact]
    public void ValidateCredentials_Should_Returns_Senha_Invalida()
    {
        // Arrange
        var controleAcesso = ControleAcessoFaker.Instance.GetNewFaker();
        controleAcesso.Usuario.StatusUsuario = StatusUsuario.Ativo;

        _repositorioMock.Setup(repo => repo.IsValidPassword(controleAcesso.Login, controleAcesso.Senha)).Returns(false);
        _repositorioMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<ControleAcesso, bool>>>())).Returns(controleAcesso);

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials(new LoginDto() { Email = controleAcesso.Login, Senha = controleAcesso.Senha });

        // Assert
        Assert.False(result.Authenticated);
        Assert.Contains("Senha inválida!", result.Message);
    }

    [Fact]
    public void ValidateCredentials_Should_Returns_Usuario_Invalido()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "teste@teste.com", Senha = "teste", };

        var usuario = new Usuario
        {
            Id = 1,
            Email = "teste@teste.com",
            StatusUsuario = StatusUsuario.Ativo
        };

        _repositorioMock.Setup(repo => repo.IsValidPassword(loginDto.Email, loginDto.Senha)).Returns(true);
        _repositorioMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<ControleAcesso, bool>>>())).Throws(new ArgumentException("Usuário Inválido!"));

        // Act &  Assert
        Assert.Throws<ArgumentException>(() => _controleAcessoBusiness.ValidateCredentials(loginDto));
    }

    [Fact]
    public void RecoveryPassword_Should_Execute_And_Returns_True()
    {
        /*
        // Arrange
        string email = "teste@example.com";
        _repositorioMock.Setup(repo => repo.RecoveryPassword(email)).Returns(true);

        // Act
        bool result = _controleAcessoBusiness.RecoveryPassword(email);

        // Assert
        Assert.True(result);
        _repositorioMock.Verify(repo => repo.RecoveryPassword(email), Times.Once);
        */
    }

    [Fact]
    public void ChangePassword_Should_Execute_And_Returns_True()
    {
        // Arrange
        int idUsuario = 1;
        string newPassword = "123456789";
        _repositorioMock.Setup(repo => repo.ChangePassword(idUsuario, newPassword)).Returns(true);

        // Act
        _controleAcessoBusiness.ChangePassword(idUsuario, newPassword);

        // Assert        
        _repositorioMock.Verify(repo => repo.ChangePassword(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void ValidateCredentials_Should_Return_Authentication_Success_When_Credentials_Are_Valid()
    {
        // Arrange
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor()
        {
            Audience = "Audience",
            Issuer = "Issuer",
            Claims = new Dictionary<string, object> { { "KEY", Guid.NewGuid() } },
            Expires = DateTime.UtcNow.AddSeconds(60)
        });
        var validToken = handler.WriteToken(securityToken);

        int idUsuario = 1;        
        var baseLogin = new ControleAcesso
        {
            Id = idUsuario,
            RefreshTokenExpiry = DateTime.UtcNow.AddHours(1),
            RefreshToken = validToken,
            Usuario = UsuarioFaker.Instance.GetNewFaker()
        };
        var authenticationDto = new AuthenticationDto
        {
            RefreshToken = validToken
        };
        _repositorioMock.Setup(repo => repo.FindByRefreshToken(It.IsAny<string>())).Returns(baseLogin);
        _tokenConfigurationMock.Setup(config => config.ValidateRefreshToken(validToken)).Returns(true);

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials(validToken);

        // Assert
        Assert.True(result.Authenticated);
        Assert.NotNull(result.AccessToken);
    }

    [Fact]
    public void ValidateCredentials_Should_Revoke_Token_When_RefreshToken_Expires()
    {
        // Arrange
        int idUsuario = 1;
        var baseLogin = new ControleAcesso
        {
            Id = idUsuario,
            RefreshTokenExpiry = DateTime.UtcNow.AddHours(-1), // Token expirado
            RefreshToken = "expired_refresh_token"
        };
        var authenticationDto = new AuthenticationDto
        {
            RefreshToken = "expired_refresh_token"
        };
        _repositorioMock.Setup(repo => repo.FindByRefreshToken(It.IsAny<string>())).Returns(baseLogin);
        _tokenConfigurationMock.Setup(config => config.ValidateRefreshToken(authenticationDto.RefreshToken)).Returns(true);

        // Act
        _controleAcessoBusiness.ValidateCredentials("expired_refresh_token");

        // Assert
        _repositorioMock.Verify(repo => repo.RevokeRefreshToken(idUsuario), Times.Once);
    }

    [Fact]
    public void ValidateCredentials_Should_Return_Authentication_Exception_When_RefreshToken_Is_Invalid()
    {
        // Arrange
        var authenticationDto = new AuthenticationDto
        {
            RefreshToken = "invalid_refresh_token"
        };
        _tokenConfigurationMock.Setup(config => config.ValidateRefreshToken(authenticationDto.RefreshToken)).Returns(false);

        // Act
        var result = _controleAcessoBusiness.ValidateCredentials("invalid_refresh_token");

        // Assert
        Assert.False(result.Authenticated);
        Assert.Equal("Refresh Token Inválido!", result.Message);
    }

    [Fact]
    public void ValidateCredentials_Should_Revoke_Token_When_RefreshToken_Is_Invalid()
    {
        // Arrange
        var mockControleAcesso = ControleAcessoFaker.Instance.GetNewFaker();
        _repositorioMock.Setup(repo => repo.FindByRefreshToken(It.IsAny<string>())).Returns(mockControleAcesso);
        _tokenConfigurationMock.Setup(config => config.ValidateRefreshToken(It.IsAny<string>())).Returns(false);

        // Act
        _controleAcessoBusiness.ValidateCredentials("invalid_refresh_token");

        // Assert
        _repositorioMock.Verify(repo => repo.RevokeRefreshToken(It.IsAny<int>()), Times.Once);
    }

}