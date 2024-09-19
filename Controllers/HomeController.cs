using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmakApp.Data;
using SmakApp.Models;
using System.Linq;

namespace SmakApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Dependency Injection ile veritaban� ba�lam�n� al�yoruz
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Anasayfa i�in GET iste�i
        [HttpGet]
        public IActionResult Index()
        {
            // Session'dan kullan�c� e-postas� ve ismini al�yoruz
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userName = HttpContext.Session.GetString("UserName");

            // Kullan�c� bilgilerini ViewData ile view'a iletiyoruz
            ViewData["UserEmail"] = userEmail;
            ViewData["UserName"] = userName;

            // Kullan�c� oturum a�m��sa
            if (userEmail != null)
            {
                // Kullan�c� ve �irket bilgilerini veritaban�ndan getiriyoruz
                var user = _context.Users.Include(u => u.Company)
                    .SingleOrDefault(u => u.Email == userEmail);

                if (user != null)
                {
                    // �irket ad� ve bakiyesini ViewData ile iletiyoruz
                    var companyName = user.Company?.Name;
                    var companyBalance = user.Company?.Balance;

                    ViewData["CompanyName"] = companyName;
                    ViewData["CompanyBalance"] = companyBalance;
                }
            }

            // Anasayfa view'�n� d�nd�r�yoruz
            return View();
        }

        // �r�nler sayfas�n� g�steren GET metodu
        [HttpGet]
        public IActionResult Product()
        {
            // T�m �r�nleri veritaban�ndan al�yoruz
            var products = _context.Products.ToList();

            // ProductViewModel kullanarak �r�nleri view'a iletiyoruz
            var model = new ProductViewModel
            {
                Products = products
            };

            return View(model);
        }

        // Yeni �r�n ekleme i�lemi 
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            // Model ge�erliyse �r�n� ekleyip veritaban�na kaydediyoruz
            if (ModelState.IsValid)
            {
                _context.Products.Add(product); // Yeni �r�n ekleme
                _context.SaveChanges(); // De�i�iklikler kaydediliyor

                // �r�nler sayfas�na y�nlendirilme
                return RedirectToAction("Product");
            }

            // Model ge�erli de�ilse �r�nler sayfas�n� tekrar g�ster
            var products = _context.Products.ToList();
            var model = new ProductViewModel
            {
                Products = products
            };
            return View("Product", model);
        }

        // �r�n silme i�lemi 
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            // Silinecek �r�n� veritaban�ndan buluyoruz
            var product = _context.Products.Find(productId);

            if (product != null)
            {
                // �r�n bulunduysa veritaban�ndan siliyoruz
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            // �r�nler sayfas�na y�nlendirme
            return RedirectToAction("Product");
        }

        // �r�n d�zenleme sayfas�n� g�sterme
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            // D�zenlenecek �r�n� veritaban�ndan buluyoruz
            var product = _context.Products.Find(id);

            if (product == null)
            {
                // �r�n bulunamazsa 404 d�nd�r�r
                return NotFound();
            }

            // D�zenleme view'�n� d�nd�r�yoruz
            return View(product);
        }

        // �r�n g�ncelleme i�lemi
        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            // Model ge�erliyse �r�n� g�ncelleyip kaydediyoruz
            if (ModelState.IsValid)
            {
                _context.Products.Update(product); // �r�n� g�ncelle
                _context.SaveChanges(); 

                // �r�nler sayfas�na y�nlendiriliyor
                return RedirectToAction("Product");
            }

            // Model ge�erli de�ilse d�zenleme sayfas�n� tekrar g�ster
            return View("EditProduct", product);
        }
    }
}
