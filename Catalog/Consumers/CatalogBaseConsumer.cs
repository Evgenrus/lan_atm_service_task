using Catalog.Service;

namespace Catalog.Consumers;

public class CatalogBaseConsumer
{
    protected readonly ICatalogService CatalogsService;
    public CatalogBaseConsumer(ICatalogService catalogService)
    {
        CatalogsService = catalogService;
    }
}