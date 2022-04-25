using Infrastructure.Models;

namespace Catalog.Service;

public interface ICatalogService
{
    public Task<ICollection<ProductModel>> AddProductsToCart(ICollection<ProductModel> productModels);
    public Task<ICollection<ProductModel>> OrderProducts(ICollection<ProductModel> productModels);

    public Task<ProductModel> ItemById(int id);
    public Task<ProductModel> ItemByName(string name);
    public Task<ICollection<ProductModel>> ItemsByBrandName(string name);
    public Task NewItem(ProductModel model);
    public Task NewBrand(BrandModel model);
}