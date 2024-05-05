﻿using despesas_backend_api_net_core.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Api.Controllers;
public class AuthControllerTest
{
    private readonly AuthController _authController;

    public AuthControllerTest()
    {
        _authController = new Mock<AuthController>().Object;
    }

    private void SetupBearerToken(int userId)
    {
        var claims = new List<Claim>
        {
            new Claim("IdUsuario", userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Bearer");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContext.Request.Headers["Authorization"] = "Bearer " + GenerateJwtToken(userId);

        _authController.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    private string GenerateJwtToken(int userId)
    {

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        var key = Guid.NewGuid();
        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("IdUsuario", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(1),
        });
        return handler.WriteToken(securityToken);
    }

    [Fact]
    public void IdUsuario_ShouldReturnCorrectUserId()
    {
        // Arrange
        const int mockIdUsuario = 22;
        SetupBearerToken(mockIdUsuario);

        // Act
        var result = GetProtectedProperty<int>(_authController, "IdUsuario");

        // Assert
        Assert.Equal(mockIdUsuario, result);
    }

    private T GetProtectedProperty<T>(object obj, string propertyName)
    {
        var propertyInfo = obj.GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);

        return (T)propertyInfo.GetValue(obj);
    }
}