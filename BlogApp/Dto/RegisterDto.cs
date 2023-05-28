using System.ComponentModel.DataAnnotations;

namespace BlogApp.Dto;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = null!;
    [DataType(DataType.EmailAddress)]
    [Required]

    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = null!;
}