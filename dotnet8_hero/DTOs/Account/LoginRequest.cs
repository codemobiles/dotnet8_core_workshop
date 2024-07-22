using System.ComponentModel.DataAnnotations;

namespace dotnet8_hero.DTOs.Account
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;


    }
}