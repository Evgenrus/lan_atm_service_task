using Catalog.Consumers.Requests;
using Catalog.Service;
using Infrastructure.Models;
using MassTransit;

namespace Catalog.Consumers;

public class AddToCartConsumer : CatalogBaseConsumer, IConsumer<AddToCartRequest>
{
    private readonly IBusControl _busControl;
    private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/checkProductQueue");
    
    public AddToCartConsumer(ICatalogService catalogService,
        IBusControl busControl) : base(catalogService)
    {
        _busControl = busControl;
    }

    public async Task Consume(ConsumeContext<AddToCartRequest> context)
    {
        var prods = await CatalogsService.AddProductsToCart(context.Message.Products);
        await context.RespondAsync(prods);
    }
}