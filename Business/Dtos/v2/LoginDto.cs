using Business.Dtos.Core;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos.v2;
public class LoginDto: BaseLoginDto
{
    [EmailAddress(ErrorMessage = "O campo Email � inv�lido.")]
    [Required(ErrorMessage = "O campo Email � obrigat�rio.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo Senha � obrigat�rio.")]
    public string? Senha { get; set; }
}