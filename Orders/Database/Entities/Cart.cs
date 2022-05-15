namespace Orders.Database.Entities;

public class Cart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public List<CartItem> Items { get; set; } = new List<CartItem>();
}