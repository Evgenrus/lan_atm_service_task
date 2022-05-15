using System.Text.Json.Serialization;

namespace Orders.Database.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    [JsonIgnore]
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [JsonIgnore]
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}