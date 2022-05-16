namespace Infrastructure.Models;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Article { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
    public int Count { get; set; }
    public string? Descr { get; set; }
}