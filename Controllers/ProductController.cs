using Microsoft.AspNetCore.Mvc;
using SmakApp.Data;
using SmakApp.Models;
using System.Linq;

namespace SmakApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor: Veritabanı bağlamı dependency injection ile alınıyor
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // Ürünleri listeleyen metod
        public IActionResult Index()
        {
            // Tüm ürünleri veritabanından alıyoruz ve view'a gönderiyoruz
            var products = _context.Products.ToList();
            return View(products); // Ürünler view'ını döndürüyoruz
        }

        // Yeni ürün oluşturma formunu görüntüleyen metot 
        public IActionResult Create()
        {
            return View(); // Ürün oluşturma formu view'ını döndürüyoruz
        }

        // Yeni ürün ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public IActionResult Create(Product product)
        {
            // Form verileri geçerliyse
            if (ModelState.IsValid)
            {
                _context.Products.Add(product); // Yeni ürünü veritabanına ekliyoruz
                _context.SaveChanges(); // Değişiklikleri kaydediyoruz
                return RedirectToAction(nameof(Index)); // Ürün listesine geri dönüyoruz
            }

            // Geçersizse, formu tekrar gösteriyoruz
            return View(product);
        }

        // Ürün düzenleme formunu görüntüleyen metot
        public IActionResult Edit(int id)
        {
            // Düzenlenecek ürünü veritabanından buluyoruz
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 sayfası döndürür
            }
            return View(product); // Ürün düzenleme formunu döndürüyoruz
        }

        // Ürün güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public IActionResult Edit(Product product)
        {
            // Form verileri geçerliyse
            if (ModelState.IsValid)
            {
                _context.Products.Update(product); // Ürünü güncelliyoruz
                _context.SaveChanges(); // Değişiklikleri kaydediyoruz
                return RedirectToAction(nameof(Index)); // Ürün listesine geri dönüyoruz
            }

            // Geçersizse, formu tekrar gösteriyoruz
            return View(product);
        }

        // Ürün silme sayfasını görüntüleyen metot 
        public IActionResult Delete(int id)
        {
            // Silinecek ürünü veritabanından buluyoruz
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 sayfası döndürür
            }
            return View(product); // Silme onayı sayfasını döndürüyoruz
        }

        // Ürün silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public IActionResult DeleteConfirmed(int id)
        {
            // Silinecek ürünü veritabanından buluyoruz
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product); // Ürünü veritabanından siliyoruz
                _context.SaveChanges(); // Değişiklikleri kaydediyoruz
            }
            return RedirectToAction(nameof(Index)); // Ürün listesine geri dönüyoruz
        }
    }
}
