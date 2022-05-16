using Delivery.Data;
using Delivery.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Services;

public class DeliveryService : IDeliveryService
{
    private DeliveryDbContext _context { get; set; }

    public DeliveryService(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<OrderDelivery>> GetAllFreeOrders()
    {
        var orders = await _context.Deliveries
            .Where(x => x.CourierId == null).ToListAsync();
        return orders;
    }

    public async Task BeginOrderDelivery(int orderId, int courierId)
    {
        var order = await _context.Deliveries
            .SingleOrDefaultAsync(x => x.OrderDeliveryId == orderId);
        if (order is null)
        {
            throw new ArgumentException("Wrong orderId");
        }

        if (order.Courier is not null)
        {
            throw new NotImplementedException("this order is already delivering");
        }

        var courier = await _context.Couriers
            .FirstOrDefaultAsync(x => x.CourierId == courierId);

        if (courier is null)
        {
            throw new ArgumentException("This courier doesn't exists");
        }

        order.Courier = courier;
        order.CourierId = courier.CourierId;
        order.Status = Data.Entities.DeliveryStatus.Assigned;

        courier.Deliveries.Add(order);

        _context.Entry(order).State = EntityState.Modified;
        _context.Entry(courier).State = EntityState.Modified;

        _context.SaveChanges();
    }

    public async Task ReceiveOrderFromStock(int orderId, int courierId)
    {
        var order = await _context.Deliveries
            .SingleOrDefaultAsync(x => x.OrderDeliveryId == orderId);
        if (order is null)
        {
            throw new ArgumentException("Wrong orderId");
        }

        if (order.CourierId <= 0)
        {
            throw new NotImplementedException("this order is not assigned to any courier");
        }

        if (order.Status != Data.Entities.DeliveryStatus.Assigned)
        {
            throw new NotImplementedException("Wrong status. required status 'Assigned';");
        }
        order.Status = Data.Entities.DeliveryStatus.Taken;

        _context.SaveChanges();
    }

    public async Task FinishDelivery(int orderId)
    {
        var order = await _context.Deliveries
            .SingleOrDefaultAsync(x => x.OrderDeliveryId == orderId);
        if (order is null)
        {
            throw new ArgumentException("Wrong orderId");
        }

        if (order.CourierId <= 0)
        {
            throw new NotImplementedException("this order is not assigned to any courier");
        }

        if (order.Status != Data.Entities.DeliveryStatus.Taken)
        {
            throw new NotImplementedException("Wrong status. required status 'Taken';");
        }
        order.Status = Data.Entities.DeliveryStatus.Finished;

        await _context.SaveChangesAsync();
    }

    public async Task<OrderDelivery?> DeliveryStatus(int orderId)
    {
        var order = await _context.Deliveries
            .SingleOrDefaultAsync(x => x.OrderDeliveryId == orderId);
        if (order is null)
        {
            throw new NotImplementedException("wrong orderId");
        }

        return order;
    }
}