using Microsoft.AspNetCore.Mvc;
using SmakApp.Data;
using SmakApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmakApp.Services;

namespace SmakApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IInvitationService _invitationService;

        // Dependency Injection (DI) ile veritabanı bağlamı ve davet hizmeti alınıyor
        public AccountController(AppDbContext context, IInvitationService invitationService)
        {
            _context = context;
            _invitationService = invitationService;
        }

        // Login sayfasını GET isteği ile döndürür
        [HttpGet]
        public IActionResult Login()
        {
            // Login sayfasını döndürür
            return View();
        }

        // Login işlemi 
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // Modelin doğruluğunu kontrol eder
            {
                // Kullanıcıyı e-posta adresine göre bulur
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                // Kullanıcı var mı ve şifre doğru mu kontrol eder
                if (user != null && user.Password == model.Password)
                {
                    //Session'a kullanıcı bilgilerini kaydeder
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", user.Name);

                    TempData["LoginMessage"] = "Giriş Yapıldı"; // Giriş mesajı
                    return RedirectToAction("Index", "Home"); // Anasayfaya yönlendirir
                }
                else
                {
                    // Hatalı giriş işlemi: Hata mesajını gösterir
                    ViewBag.LoginError = "Yanlış e-posta veya şifre."; // Hata mesajı
                    return View(model); // Aynı sayfaya geri döner
                }
            }

            // Model geçersizse, tekrar login sayfasını döndürür
            return View(model);
        }

        // Şifre doğrulama işlemi 
        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return inputPassword == storedPasswordHash; // Şifre kontrolü 
        }

        // Çıkış işlemi
        [HttpGet]
        public IActionResult Logout()
        {
            // Session'dan kullanıcı bilgilerini temizler
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserName");

            TempData["LogoutMessage"] = "Çıkış Yapıldı"; // Çıkış mesajı

            return RedirectToAction("Index", "Home"); // Anasayfaya yönlendirir
        }

        // Kayıt formunu GET isteği ile döndürür (davet ile gelirse e-posta ve şirket adı dolu gelir)
        [HttpGet]
        public IActionResult Register(string email = "", string companyName = "")
        {
            var model = new RegisterViewModel
            {
                Email = email,
                CompanyName = companyName
            };
            return View(model);
        }

        // Kayıt işlemi
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Model geçerliyse
            {
                // Aynı e-posta adresini kullanan başka bir kullanıcı olup olmadığını kontrol eder
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (existingUser != null) // Kullanıcı zaten varsa hata mesajı ekler
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılmaktadır.");
                    return View(model); // Aynı sayfaya geri döner
                }

                // Yeni şirket oluşturur
                var company = new Company
                {
                    Name = model.CompanyName,
                    Balance = model.Balance
                };

                _context.Companies.Add(company); // Şirketi veritabanına ekler
                _context.SaveChanges(); // Değişiklikleri kaydeder

                // Yeni kullanıcı oluşturur ve şirkete atar
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    CompanyId = company.Id
                };

                _context.Users.Add(user); // Kullanıcıyı veritabanına ekler
                _context.SaveChanges();

                TempData["RegisterMessage"] = "Kayıt Başarılı";

                // Giriş yapmış kullanıcı olarak yönlendirme yapılabilir
                return RedirectToAction("Index", "Home"); // Anasayfaya yönlendirir
            }

            // Model geçersizse, aynı sayfaya geri döner
            return View(model);
        }

        // Davet formu
        [HttpGet]
        public IActionResult Invite()
        {
            return View(); 
        }

        // Davet işlemi
        [HttpPost]
        public IActionResult Invite(string email)
        {
            // E-posta adresinin kayıtlı olup olmadığını kontrol eder
            var userExists = _context.Users.Any(u => u.Email == email);

            if (userExists)
            {
                ViewData["ErrorMessage"] = "Bu e-posta adresi zaten kayıtlı.";
                return View();
            }

            // E-posta adresinin daha önce davet edilip edilmediğini kontrol eder
            var invitationExists = _context.Invitations.Any(i => i.Email == email);

            if (invitationExists)
            {
                ViewData["ErrorMessage"] = "Bu e-posta adresi daha önce davet edilmiştir.";
                return View();
            }

            // Oturumdan kullanıcı e-postasını alır
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.Include(u => u.Company)
                .SingleOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Davetiye oluşturur ve veritabanına ekler
            var invitation = new Invitation
            {
                Email = email,
            };

            _context.Invitations.Add(invitation);
            _context.SaveChanges();

            // Davet başarılı mesajı
            TempData["InviteMessage"] = "Davet başarıyla gönderildi!";

            // Davet edilen kişiyi kayıt sayfasına yönlendirir
            return RedirectToAction("Register", "Account", new { email, companyName = user.Company?.Name });
        }
    }
}
