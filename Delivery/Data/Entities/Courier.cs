namespace Delivery.Data.Entities;

public class Courier
{
    public int CourierId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public virtual List<OrderDelivery> Deliveries { get; set; } = new List<OrderDelivery>();
}