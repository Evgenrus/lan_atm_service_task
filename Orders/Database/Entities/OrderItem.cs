using System.Text.Json.Serialization;

namespace Orders.Database.Entities;

public class OrderItem
{
    public int OrderItemId { get; set; }
    
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }

    public int ItemId { get; set; }
    [JsonIgnore]
    public Item Item { get; set; }
    public int Count { get; set; }
}