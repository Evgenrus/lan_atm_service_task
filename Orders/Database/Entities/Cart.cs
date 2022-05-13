using System.Text.Json.Serialization;

namespace Orders.Database.Entities;

public class Cart
{
    public int CartId { get; set; }

    //public int CustomerId { get; set; }
    [JsonIgnore]
    //public Customer Customer { get; set; }

    public List<CartItem> Items { get; set; } = new List<CartItem>();
}