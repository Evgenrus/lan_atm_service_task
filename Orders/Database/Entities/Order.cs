namespace Orders.Database.Entities;

public class Order
{
    public int OrderId { get; set; }
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}