using SmakApp.Models;

public class Product
{
    // Ürünün benzersiz kimliði
    public int Id { get; set; }

    // Ürünün adý
    public string Name { get; set; }

    // Ürünün açýklamasý
    public string Description { get; set; }

    // Ürünün fiyatý
    public decimal Price { get; set; }

    // Ürün ile iliþkilendirilmiþ CompanyProduct nesneleri
    public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();
}
