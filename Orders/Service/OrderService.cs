using System.Net;
using System.Runtime.InteropServices;
using Infrastructure.Models;
using Infrastructure.PostForms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Orders.Database;
using Orders.Database.Entities;
using System.Text.Json;
using Infrastructure.Helpers;

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
        return result;
     }

    public async Task AddItemToCart(int cartId, Item item, int count)
    {
        var cart = await _context.Carts.SingleOrDefaultAsync(x => x.CartId == cartId) ?? new Cart();

        var httpHelper = new HttpRequestHelper(
            "http://localhost:7002/api/v1/Catalog/CheckItem", HttpMethodsTypes.Post, item);

        var response = await httpHelper.ExecuteRequest();

        response.EnsureSuccessStatusCode();

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
        if (!cartItems.Any())
        {
            throw new NotImplementedException("Order is empty");
        }

        var orderItems = await CartItemsToOrderItems(cartItems, order);
        if (!orderItems.Any())
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

    public async Task NewCustomer(NewCustomerModel model)
    {
        var customer = new Customer
        {
            Address = model.Address,
            Mail = model.Mail,
            Name = model.Name,
            Surname = model.Surname,
            Phone = model.Phone,
            Orders = new List<Order>()
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItem>> CartItems(int id)
    {
        var cartWithItems = _context.Carts.Include(x => x.Items);
        var cart = await cartWithItems.FirstOrDefaultAsync(x => x.CartId == id);
        if(cart is null)
        {
            throw new Exception("Wrong cart id");
        }

        if(cart.Items is null)
        {
            throw new Exception("Empty cart");
        }

        return cart.Items;
    }
}
