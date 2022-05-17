using Delivery.Data;
using Delivery.Data.Entities;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Text.Json;
using System.Net;

namespace Delivery.Services;

public class DeliveryService : IDeliveryService
{
    private DeliveryDbContext _context { get; set; }

    public DeliveryService(DeliveryDbContext context)
    {
        _context = context;
    }


    public async Task PostNewDelivery(DeliveryInfo delivery)
    {
        var deliveryorder = new OrderDelivery
        {
            Address = delivery.Address,
            Status = 0,
            Receiver = delivery.Receiver
        };
        
        var deliveryItems = new List<DeliveryItem>();
        if (delivery.ItemsInOrder is null)
        {
            throw new Exception("Order is empty");
        }
        foreach (var item in delivery.ItemsInOrder)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.ConnectionClose = true;

            var itemJson = JsonSerializer.Serialize(item);

            var requestContent = new StringContent(itemJson, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:7002/api/v1/Catalog/CheckItem", requestContent);

            response.EnsureSuccessStatusCode();

            var delItem = new DeliveryItem
            {
                Article = item.Article,
                Count = item.Count,
                Name = item.Name,
                OrderDelivery = deliveryorder
            };
            deliveryItems.Add(delItem);
        }
        
        deliveryorder.OrderedItems.AddRange(deliveryItems);
        
        await _context.Deliveries.AddAsync(deliveryorder);
        await _context.DeliveryItems.AddRangeAsync(deliveryItems);

        _context.SaveChanges();
    }

    public async Task<ICollection<OrderDelivery>> GetAllFreeOrders()
    {
        var orders = await _context.Deliveries
            .Where(x => x.CourierId == null)
            .Include(x => x.OrderedItems)
            .ToListAsync();
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

    public async Task<DeliveryInfo> DeliveryStatus(int orderId)
    {
        var nav =_context.Deliveries.Include(x => x.OrderedItems);
        var order = await nav
            .SingleOrDefaultAsync(x => x.OrderDeliveryId == orderId);
        if (order is null)
        {
            throw new NotImplementedException("wrong orderId");
        }

        var items = DeliveryItemsToItems(order.OrderedItems);

        var info = new DeliveryInfo
        {
            Address = order.Address,
            DeliveryId = order.OrderDeliveryId,
            ItemsInOrder = items,
            Receiver = order.Receiver,
            Status = order.Status.GetDisplayName()
        };

        return info;
    }

    private List<Item> DeliveryItemsToItems(List<DeliveryItem> deliveryItems)
    {
        var result = new List<Item>();
        foreach (var deliveryItem in deliveryItems)
        {
            var item = new Item
            {
                Article = deliveryItem.Article,
                Count = deliveryItem.Count,
                Name = deliveryItem.Name
            };
            result.Add(item);
        }

        return result;
    }
}