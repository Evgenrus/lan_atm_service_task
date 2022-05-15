using Microsoft.EntityFrameworkCore;
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
    
    public async Task<ICollection<Item>> GetAllItems()
    {
        var result = await _context.Items.ToListAsync();
        return result;
    }

    public async Task<ICollection<Item>> GetItemById(int id)
    {
        var result = await _context.Items.Where(x => x.ItemId == id).ToListAsync();
        return result;
    }

    public async Task AddItemToCart(int cartId, Item item)
    {
        var cart = await _context.Carts.SingleOrDefaultAsync(x => x.CartId == cartId);
        if (cart == null)
        {
            throw new ArgumentException("No such cart");
        }
        
        
    }
}