using Business.Dtos.Core;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos.v2;
public class LoginDto: BaseLoginDto
{
    [EmailAddress(ErrorMessage = "O campo Email � inv�lido.")]
    [Required(ErrorMessage = "O campo Email � obrigat�rio.")]
    public override string? Email { get; set; }

    [Required(ErrorMessage = "O campo Senha � obrigat�rio.")]
    public override string? Senha { get; set; }
}