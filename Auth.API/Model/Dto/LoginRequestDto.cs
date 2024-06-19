using System.ComponentModel.DataAnnotations;

namespace Auth.API.Model.Dto;

public class LoginRequestDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
