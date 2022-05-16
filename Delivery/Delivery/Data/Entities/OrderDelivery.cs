using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Delivery.Data.Entities;

public enum DeliveryStatus : int
{
    Free,
    Assigned,
    Taken,
    Finished
}

public class OrderDelivery
{
    public int OrderDeliveryId { get; set; }

    public int? CourierId { get; set; }
    [JsonIgnore] public virtual Courier? Courier { get; set; }

    public string Address { get; set; }

    public DeliveryStatus Status { get; set; } = DeliveryStatus.Free;

    public virtual List<DeliveryItem> OrderedItems { get; set; } = new List<DeliveryItem>();
}