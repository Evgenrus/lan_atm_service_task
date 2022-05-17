namespace Infrastructure.Models;

public class DeliveryInfo
{
    public int DeliveryId { get; set; }
    public string Status { get; set; }
    public string Address { get; set; }
    public string Receiver { get; set; }
    public List<Item>? ItemsInOrder { get; set; }
}