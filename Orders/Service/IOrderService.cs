using Orders.Database.Entities;

namespace Orders.Service;

public interface IOrderService
{
    public Task<ICollection<Item>> GetAllItems();
    public Task<ICollection<Item>> GetItemById(int id);
    public Task AddItemToCart(int cartId, Item item);
}