using SmakApp.Models;

public class Product
{
    // �r�n�n benzersiz kimli�i
    public int Id { get; set; }

    // �r�n�n ad�
    public string Name { get; set; }

    // �r�n�n a��klamas�
    public string Description { get; set; }

    // �r�n�n fiyat�
    public decimal Price { get; set; }

    // �r�n ile ili�kilendirilmi� CompanyProduct nesneleri
    public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();
}
