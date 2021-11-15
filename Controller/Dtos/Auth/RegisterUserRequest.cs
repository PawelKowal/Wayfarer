using System.ComponentModel.DataAnnotations;

namespace Controller.Dtos.Auth
{
    public class RegisterUserRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Your name must contain less than 50 characters.")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Your email must contain less than 50 characters.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Your password must contain less than 50 characters.")]
        public string Password { get; set; }
    }
}
