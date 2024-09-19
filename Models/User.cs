using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class User
    {
        [Key] // Bu özellik, User tablosundaki primary key olarak belirlenir
        public int Id { get; set; }

        [Required] // Bu özellik, kullanıcı adı alanının zorunlu olduğunu belirtir
        [MaxLength(100)] // Kullanıcı adının en fazla 100 karakter olabileceğini belirtir
        public string Name { get; set; }

        [Required] // Bu özellik, e-posta alanının zorunlu olduğunu belirtir
        [EmailAddress] // E-posta formatının geçerli olduğunu doğrular
        public string Email { get; set; }

        [Required] // Bu özellik, şifre alanının zorunlu olduğunu belirtir
        [DataType(DataType.Password)] // Şifre olarak biçimlendirilmiş olduğunu belirtir
        public string Password { get; set; }

        public int CompanyId { get; set; } // Kullanıcının ait olduğu şirketin ID'si

        public Company Company { get; set; } // İlişkili Company nesnesi
    }
}
