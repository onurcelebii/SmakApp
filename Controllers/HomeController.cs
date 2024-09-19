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

        // Dependency Injection ile veritabaný baðlamýný alýyoruz
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Anasayfa için GET isteði
        [HttpGet]
        public IActionResult Index()
        {
            // Session'dan kullanýcý e-postasý ve ismini alýyoruz
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userName = HttpContext.Session.GetString("UserName");

            // Kullanýcý bilgilerini ViewData ile view'a iletiyoruz
            ViewData["UserEmail"] = userEmail;
            ViewData["UserName"] = userName;

            // Kullanýcý oturum açmýþsa
            if (userEmail != null)
            {
                // Kullanýcý ve þirket bilgilerini veritabanýndan getiriyoruz
                var user = _context.Users.Include(u => u.Company)
                    .SingleOrDefault(u => u.Email == userEmail);

                if (user != null)
                {
                    // Þirket adý ve bakiyesini ViewData ile iletiyoruz
                    var companyName = user.Company?.Name;
                    var companyBalance = user.Company?.Balance;

                    ViewData["CompanyName"] = companyName;
                    ViewData["CompanyBalance"] = companyBalance;
                }
            }

            // Anasayfa view'ýný döndürüyoruz
            return View();
        }

        // Ürünler sayfasýný gösteren GET metodu
        [HttpGet]
        public IActionResult Product()
        {
            // Tüm ürünleri veritabanýndan alýyoruz
            var products = _context.Products.ToList();

            // ProductViewModel kullanarak ürünleri view'a iletiyoruz
            var model = new ProductViewModel
            {
                Products = products
            };

            return View(model);
        }

        // Yeni ürün ekleme iþlemi 
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            // Model geçerliyse ürünü ekleyip veritabanýna kaydediyoruz
            if (ModelState.IsValid)
            {
                _context.Products.Add(product); // Yeni ürün ekleme
                _context.SaveChanges(); // Deðiþiklikler kaydediliyor

                // Ürünler sayfasýna yönlendirilme
                return RedirectToAction("Product");
            }

            // Model geçerli deðilse ürünler sayfasýný tekrar göster
            var products = _context.Products.ToList();
            var model = new ProductViewModel
            {
                Products = products
            };
            return View("Product", model);
        }

        // Ürün silme iþlemi 
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            // Silinecek ürünü veritabanýndan buluyoruz
            var product = _context.Products.Find(productId);

            if (product != null)
            {
                // Ürün bulunduysa veritabanýndan siliyoruz
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            // Ürünler sayfasýna yönlendirme
            return RedirectToAction("Product");
        }

        // Ürün düzenleme sayfasýný gösterme
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            // Düzenlenecek ürünü veritabanýndan buluyoruz
            var product = _context.Products.Find(id);

            if (product == null)
            {
                // Ürün bulunamazsa 404 döndürür
                return NotFound();
            }

            // Düzenleme view'ýný döndürüyoruz
            return View(product);
        }

        // Ürün güncelleme iþlemi
        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            // Model geçerliyse ürünü güncelleyip kaydediyoruz
            if (ModelState.IsValid)
            {
                _context.Products.Update(product); // Ürünü güncelle
                _context.SaveChanges(); 

                // Ürünler sayfasýna yönlendiriliyor
                return RedirectToAction("Product");
            }

            // Model geçerli deðilse düzenleme sayfasýný tekrar göster
            return View("EditProduct", product);
        }
    }
}
