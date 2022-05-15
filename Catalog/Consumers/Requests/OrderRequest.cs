using Infrastructure.Models;

namespace Catalog.Consumers.Requests;

public class OrderRequest
{
    public User User { get; set; }
    public ICollection<ProductModel> Products { get; set; }
}