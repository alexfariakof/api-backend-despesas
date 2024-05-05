﻿using Business.Dtos.Parser;

namespace Business.Dtos;
public class ReceitaDtoTest
{
    [Fact]
    public void ReceitaDto_Should_Set_Properties_Correctly()
    {
        // Arrange and Act
        var receita = ReceitaFaker.Instance.Receitas().First();
        var receitaDto = new ReceitaParser().Parse(receita);

        // Assert
        Assert.Equal(receita.Id, receitaDto .Id);
        Assert.Equal(receita.Data, receitaDto .Data);
        Assert.Equal(receita.Descricao, receitaDto .Descricao);
        Assert.Equal(receita.Valor, receitaDto .Valor);
        Assert.Equal(receita.UsuarioId, receitaDto .IdUsuario);
        Assert.Equal(receita.Categoria.Id, receitaDto.Categoria.Id);
    }
}