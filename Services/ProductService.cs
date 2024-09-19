using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmakApp.Data;
using SmakApp.Models;

namespace SmakApp.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        // Dependency Injection ile veritabanı bağlamını alıyoruz
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm ürünleri döndüren metot
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList(); // Ürünleri liste olarak döndürür
        }

        // Verilen ID'ye sahip ürünü döndüren metot
        public Product GetProductById(int id)
        {
            return _context.Products.Find(id); // ID ile ürünü bulur ve döndürür
        }

        // Yeni bir ürün ekleyen metot
        public void AddProduct(Product product)
        {
            _context.Products.Add(product); // Yeni ürünü ekler
            _context.SaveChanges(); // Değişiklikleri veritabanına kaydeder
        }

        // Var olan ürünü güncelleyen metot
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product); // Ürünü günceller
            _context.SaveChanges(); // Değişiklikleri veritabanına kaydeder
        }

        // Verilen ID'ye sahip ürünü silen metot
        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id); // ID ile ürünü bulur
            if (product != null)
            {
                _context.Products.Remove(product); // Ürünü siler
                _context.SaveChanges(); // Değişiklikleri veritabanına kaydeder
            }
        }
    }
}
