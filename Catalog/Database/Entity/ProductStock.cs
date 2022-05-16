namespace Catalog.Database.Entity;

public class ProductStock
{
    public int ProductStockId { get; set; }
    public int Stock { get; set; }

    public int ProductId { get; set; }
    public Product Products { get; set; }
}