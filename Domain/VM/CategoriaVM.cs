﻿using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.VM;
public class CategoriaVM : BaseModelVM
{
    [Required]
    public string? Descricao { get; set; }    
    [Required]
    public int IdTipoCategoria { get; set; }
    public TipoCategoria TipoCategoria { get { return (TipoCategoria)IdTipoCategoria; } set   {  IdTipoCategoria = (int)value;  } }
}