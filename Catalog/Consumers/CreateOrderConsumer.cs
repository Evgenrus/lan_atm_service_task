using Catalog.Consumers.Requests;
using Catalog.Service;
using MassTransit;

namespace Catalog.Consumers;

public class CreateOrderConsumer : CatalogBaseConsumer, IConsumer<OrderRequest>
{
    private readonly IBusControl _busControl;
    private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/checkProductQueue");
    
    public CreateOrderConsumer(ICatalogService catalogService,
        IBusControl busControl) : base(catalogService)
    {
        _busControl = busControl;
    }

    public async Task Consume(ConsumeContext<OrderRequest> context)
    {
        var prods = await CatalogsService.OrderProducts(context.Message.Products);
         await context.RespondAsync(prods);
    }
}