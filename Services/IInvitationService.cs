using System.Collections.Generic;
using SmakApp.Models;

namespace SmakApp.Services
{
    public interface IInvitationService
    {
        // Tüm davetleri döndüren metot
        IEnumerable<Invitation> GetAllInvitations();

        // Verilen ID'ye sahip daveti döndüren metot
        Invitation GetInvitationById(int id);

        // Yeni bir davet ekleyen metot
        void AddInvitation(Invitation invitation);

        // Var olan daveti güncelleyen metot
        void UpdateInvitation(Invitation invitation);

        // Verilen ID'ye sahip daveti silen metot
        void DeleteInvitation(int id);
    }
}
