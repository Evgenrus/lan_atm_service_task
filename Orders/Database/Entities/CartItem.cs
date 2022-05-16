using System.Text.Json.Serialization;

namespace Orders.Database.Entities;

public class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }
    [JsonIgnore]
    public Cart Cart { get; set; }
    
    public int Count { get; set; }
}