using System.Text.Json.Serialization;

namespace Orders.Database.Entities;

public class Order
{
    public int OrderId { get; set; }
    
    //public int CustomerId { get; set; }

    public bool IsFinished { get; set; } = false;
    public bool IsCanceled { get; set; } = false;
    [JsonIgnore]
    //public Customer Customer { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}