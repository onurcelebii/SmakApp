using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class InviteViewModel
    {
        [Required] // E-posta alanının zorunlu olduğunu belirtir
        [EmailAddress] // E-posta formatının geçerli olduğunu doğrular
        public string Email { get; set; }
    }
}
