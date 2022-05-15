using Orders.Database.Entities;

namespace Orders.Service;

public interface IOrderService
{
    public Task<ICollection<Item>> GetAllItems();
    public Task<ICollection<Item>> GetItemById(int id);

    public Task<ICollection<Order>> GetAllCustomerOrders(int customerId);
    public Task AddItemToCart(int cartId, Item item, int count);
    public Task ChangeItemCountInCart(int cartId, int cartItemId, int countChange);

    public Task CreateOrder(int cartId);
    public Task CancelOrder(int orderId);
    public Task FinishOrder(int orderId);
}