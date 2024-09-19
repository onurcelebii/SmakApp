using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class RegisterViewModel
    {
        [Required] // Ad alanının zorunlu olduğunu belirtir
        public string Name { get; set; }

        [Required] // E-posta alanının zorunlu olduğunu belirtir
        [EmailAddress] // E-posta formatının geçerli olduğunu doğrular
        public string Email { get; set; }

        [Required] // Şifre alanının zorunlu olduğunu belirtir
        public string Password { get; set; }

        [Required] // Şirket adı alanının zorunlu olduğunu belirtir
        public string CompanyName { get; set; }

        [Required] // Bakiye alanının zorunlu olduğunu belirtir
        public decimal Balance { get; set; }
    }
}
