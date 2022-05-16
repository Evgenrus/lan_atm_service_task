using Delivery.Data.Entities;
using Infrastructure.Models;

namespace Delivery.Services;

public interface IDeliveryService
{
    public Task PostNewDelivery(DeliveryInfo delivery);
    public Task<ICollection<OrderDelivery>> GetAllFreeOrders();
    public Task BeginOrderDelivery(int orderId, int courierId);
    public Task ReceiveOrderFromStock(int orderId, int courierId);
    public Task FinishDelivery(int orderId);
    public Task<DeliveryInfo> DeliveryStatus(int orderId);
}