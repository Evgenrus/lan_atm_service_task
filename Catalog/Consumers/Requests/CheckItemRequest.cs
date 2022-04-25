using Infrastructure.Models;
using Product = Catalog.Database.Entity.Product;

namespace Catalog.Consumers.Requests;

public class AddToCartRequest
{
    public User User { get; set; }
    public ICollection<ProductModel> Products { get; set; }
}