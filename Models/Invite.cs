using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class Invite
    {
        [Key] // Id alaný Primary Key olarak iþaretlenmiþ
        public int Id { get; set; }

        [Required] // Email alaný zorunlu (boþ býrakýlamaz)
        [EmailAddress] // Email formatýnýn geçerli olduðundan emin olmak için
        public string Email { get; set; }

        public bool IsAccepted { get; set; } // Davetin kabul edilip edilmediðini belirler
        public DateTime InviteDate { get; set; } // Davetin yapýldýðý tarih
    }
}
