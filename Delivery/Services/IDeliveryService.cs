using Delivery.Data.Entities;

namespace Delivery.Services;

public interface IDeliveryService
{
    public Task<ICollection<OrderDelivery>> GetAllFreeOrders();
    public Task BeginOrderDelivery(int orderId, int courierId);
    public Task ReceiveOrderFromStock(int orderId, int courierId);
    public Task FinishDelivery(int orderId);
    public Task<OrderDelivery?> DeliveryStatus(int orderId);
}