using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class LoginViewModel
    {
        [Required] // E-posta alanının zorunlu olduğunu belirtir
        public string Email { get; set; }

        [Required] // Şifre alanının zorunlu olduğunu belirtir
        public string Password { get; set; }
    }
}
