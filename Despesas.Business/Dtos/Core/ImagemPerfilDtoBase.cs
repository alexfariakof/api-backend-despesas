﻿namespace Business.Dtos.Core;
public abstract class ImagemPerfilDtoBase : ModelDtoBase
{
    public virtual string? Url { get; set; }
    public virtual string? Name { get; set; }
    public virtual string? Type { get; set; }
    public virtual string? ContentType { get; set; }
    public virtual byte[]? Arquivo { get; set; }
}