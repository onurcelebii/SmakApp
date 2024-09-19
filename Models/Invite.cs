using System.ComponentModel.DataAnnotations;

namespace SmakApp.Models
{
    public class Invite
    {
        [Key] // Id alan� Primary Key olarak i�aretlenmi�
        public int Id { get; set; }

        [Required] // Email alan� zorunlu (bo� b�rak�lamaz)
        [EmailAddress] // Email format�n�n ge�erli oldu�undan emin olmak i�in
        public string Email { get; set; }

        public bool IsAccepted { get; set; } // Davetin kabul edilip edilmedi�ini belirler
        public DateTime InviteDate { get; set; } // Davetin yap�ld��� tarih
    }
}
