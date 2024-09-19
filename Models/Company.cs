using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SmakApp.Models
{
    public class Company
    {
        [Key] // Id alanı, Primary Key olarak işaretlenmiş
        public int Id { get; set; }

        [Required] // Name alanı zorunlu (boş bırakılamaz)
        public string Name { get; set; }

        [Required] // Balance alanı zorunlu
        public decimal Balance { get; set; } // Şirketin bakiyesi, decimal türünde

        // Şirketin sahip olduğu ürünleri temsil eden koleksiyon
        public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();
    }
}
