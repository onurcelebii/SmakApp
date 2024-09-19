using SmakApp.Models;

public class CompanyProduct
{
    public int CompanyId { get; set; } // Şirketin benzersiz ID'si
    public Company Company { get; set; } // Şirket nesnesi

    public int ProductId { get; set; } // Ürünün benzersiz ID'si
    public Product Product { get; set; } // Ürün nesnesi
}
