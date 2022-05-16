using System.Runtime.InteropServices;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Orders.Database;
using Orders.Database.Entities;

namespace Orders.Service;

public class OrderService : IOrderService
{
    private OrderDbContext _context;

    public OrderService(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Order>> GetAllCustomerOrders(int customerId)
    {
        var result = await _context.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
        // var result = await _context.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
        // return result;
        throw new NotImplementedException("Not implemented");
    }
    public async Task<ICollection<Item>> GetItemById(int id)
    {
        var result = await _context.Items.Where(x => x.ItemId == id).ToListAsync();
        return result;
        //throw new NotImplementedException("Not implemented");
    }

    public async Task AddItemToCart(int cartId, Item item, int count)
    {
        var cart = await _context.Carts.SingleOrDefaultAsync(x => x.CartId == cartId) ?? new Cart();

        //TODO Check item in catalog

        //TODO check stocks request

        var itemcart = new CartItem
        {
            Cart = cart,
            CartId = cart.CartId, 
            Count = count
        };

        await _context.CartItems.AddAsync(itemcart);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeItemCountInCart(int cartId, int cartItemId, int countChange)
    {
        var cart = await _context.Carts.SingleOrDefaultAsync(x => x.CartId == cartId);
        if (cart is null)
        {
            throw new NotImplementedException("Cart doesn't exists");
        }
        var cartItem = await _context.CartItems.SingleOrDefaultAsync(x => x.CartItemId == cartItemId);
        if (cartItem is null)
        {
            throw new NotImplementedException("CartItem doesn't exists");
        }
        
        //TODO check stocks

        if (cartItem.Count + countChange < 0)
        {
            throw new ArgumentException("Items count couldn't be less than 0");
        }

        cartItem.Count += countChange;
        if (cartItem.Count == 0)
        {
            _context.CartItems.Remove(cartItem);
        }
        await _context.SaveChangesAsync();
    }

    public async Task CreateOrder(int cartId)
    {
        var order = new Order();
        await _context.Orders.AddAsync(order);

        var cart = await _context.Carts.SingleOrDefaultAsync(x => x.CartId == cartId);
        if (cart is null)
        {
            throw new NotImplementedException("Wrong cart id");
        }

        var cartItems = await _context.CartItems.Where(x => x.CartId == cart.CartId).ToListAsync();
        if (cartItems.Count <= 0)
        {
            throw new NotImplementedException("Order is empty");
        }

        var orderItems = await CartItemsToOrderItems(cartItems, order);
        if (orderItems.Count <= 0)
        {
            throw new NotImplementedException("OrderList is empty");
        }

        foreach (var item in orderItems)
        {
            item.Order = order;
            item.OrderId = order.OrderId;
        }

        order.OrderItems.AddRange(orderItems);
        await _context.SaveChangesAsync();
    }

    public async Task CancelOrder(int orderId)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(x => x.OrderId == orderId);
        if (order is null)
        {
            throw new NotImplementedException("No such order");
        }

        order.IsCanceled = true;
        await _context.SaveChangesAsync();
    }

    public async Task FinishOrder(int orderId)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(x => x.OrderId == orderId);
        if (order is null)
        {
            throw new NotImplementedException("No such order");
        }

        order.IsFinished = true;
        await _context.SaveChangesAsync();
    }

    //Convert ICollection of CartItems to ICollection of OrderItems
    private async Task<ICollection<OrderItem>> CartItemsToOrderItems(ICollection<CartItem> cartItems, Order order)
    {
        // var order = new Order();
        // await _context.Orders.AddAsync(order);
        
        var orderItems = new List<OrderItem>();
        foreach (var cartItem in cartItems)
        {
            var item = new OrderItem
            {
                Count = cartItem.Count,
                Order = order,
                OrderId = order.OrderId
            };
            orderItems.Add(item);
        }

        //order.OrderItems.AddRange(orderItems);
        await _context.SaveChangesAsync();

        return orderItems;
    }
}
