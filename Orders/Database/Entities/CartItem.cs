namespace Orders.Database.Entities;

public class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; }

    public int ItemId { get; set; }
    public Item Item { get; set; }
    
    public int Count { get; set; }
}