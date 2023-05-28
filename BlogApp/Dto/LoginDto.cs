using System.ComponentModel.DataAnnotations;

namespace BlogApp.Dto;

public class LoginDto
{
    [DataType(DataType.EmailAddress)]
    [Required]

    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; } = false;
}
