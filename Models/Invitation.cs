using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; } // Davet kaydının primary key'i

        [Required] // E-posta adresinin zorunlu olduğunu belirtir
        [EmailAddress] // E-posta formatının geçerli olduğunu doğrular
        public string Email { get; set; } // Davet edilen e-posta adresi

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Davetin oluşturulma tarihi
    }
}
