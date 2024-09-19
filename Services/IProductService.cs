using System.Collections.Generic;
using SmakApp.Models;

namespace SmakApp.Services
{
    public interface IProductService
    {
        // Tüm ürünleri döndüren metot
        IEnumerable<Product> GetAllProducts();

        // Verilen ID'ye sahip ürünü döndüren metot
        Product GetProductById(int id);

        // Yeni bir ürün ekleyen metot
        void AddProduct(Product product);

        // Var olan ürünü güncelleyen metot
        void UpdateProduct(Product product);

        // Verilen ID'ye sahip ürünü silen metot
        void DeleteProduct(int id);
    }
}
