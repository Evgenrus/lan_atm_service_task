namespace Catalog.Database.Entity;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public string Article { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int BrandId { get; set; }
    public Brand Brand { get; set; }

    public int ProductStockId { get; set; }
    public ProductStock Stock { get; set; }
}