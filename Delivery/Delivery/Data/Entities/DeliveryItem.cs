using System.Text.Json.Serialization;

namespace Delivery.Data.Entities;

public class DeliveryItem
{
    public int DeliveryItemId { get; set; }
    public string Name { get; set; }
    public string Article { get; set; }
    public int Count { get; set; }

    public int OrderDeliveryId { get; set; }
    [JsonIgnore]
    public virtual OrderDelivery OrderDelivery { get; set; }
}