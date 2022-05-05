namespace Orders.Database.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}