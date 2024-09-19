using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmakApp.Data;
using SmakApp.Models;

namespace SmakApp.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly AppDbContext _context;

        // DbContext dependency injection ile alınıyor
        public InvitationService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm davetleri döndüren metot
        public IEnumerable<Invitation> GetAllInvitations()
        {
            // Veritabanındaki tüm davetleri liste olarak alıyoruz
            return _context.Invitations.ToList();
        }

        // Belirli bir ID'ye sahip daveti döndüren metot
        public Invitation GetInvitationById(int id)
        {
            // ID'ye göre daveti veritabanından buluyoruz
            return _context.Invitations.Find(id);
        }

        // Yeni bir davet ekleyen metot
        public void AddInvitation(Invitation invitation)
        {
            // Yeni daveti veritabanına ekliyoruz
            _context.Invitations.Add(invitation);
            // Değişiklikleri veritabanına kaydediyoruz
            _context.SaveChanges();
        }

        public void UpdateInvitation(Invitation invitation)
        {
            _context.Invitations.Update(invitation);
            _context.SaveChanges();
        }

        public void DeleteInvitation(int id)
        {
            var invitation = _context.Invitations.Find(id);
            if (invitation != null)
            {
                _context.Invitations.Remove(invitation);
                _context.SaveChanges();
            }
        }
    }
}
