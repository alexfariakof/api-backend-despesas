using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Business.Dtos;
public class LoginDto
{
    [EmailAddress(ErrorMessage = "O campo Email � inv�lido.")]
    [Required(ErrorMessage = "O campo Email � obrigat�rio.")]    
    public string? Email { get; set; }
   
    [Required(ErrorMessage = "O campo Senha � obrigat�rio.")]
    public string? Senha { get; set; }
}