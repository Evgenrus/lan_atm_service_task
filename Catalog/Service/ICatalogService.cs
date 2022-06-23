using Infrastructure.Models;

namespace Catalog.Service;

public interface ICatalogService
{
    public Task<ICollection<Item>> AddProductsToCart(ICollection<Item> productModels);
    public Task<ICollection<Item>> OrderProducts(ICollection<Item> productModels);

    public Task<Item> CheckItem(Item item);
    public Task<ICollection<Item>> CheckItems(ICollection<Item> items);

    public Task<Item?> ItemById(int id);
    public Task<Item?> ItemByName(string name);
    public Task<ICollection<Item>> ItemsByBrandName(string name);
    public Task NewItem(Item model);
    public Task NewBrand(BrandModel model);
    public Task NewCategory(CategoryModel model);
}