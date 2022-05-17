namespace Infrastructure.Models;

public class CategoryModel
{
    public string Name { get; set; }

    public List<Item> Products { get; set; } = new List<Item>();
}