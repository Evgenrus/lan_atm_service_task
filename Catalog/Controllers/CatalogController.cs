using Catalog.Database;
using Catalog.Service;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CatalogController : ControllerBase
{
    private CatalogDbContext _context { get; set; }
    private ICatalogService _service { get; set; }

    public CatalogController(CatalogDbContext context, ICatalogService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemById(int id)
    {
        var prods = await _service.ItemById(id);
        return Ok(prods);
    }

    [HttpGet]
    public async Task<IActionResult> GetItemByName(string name)
    {
        var prods = await _service.ItemByName(name);
        return Ok(prods);
    }

    [HttpGet]
    public async Task<IActionResult> GetItemsByBrandName(string name)
    {
        var prods = await _service.ItemsByBrandName(name);
        return Ok(prods);
    }

    [HttpPost]
    public async Task<IActionResult> PostNewItem(ProductModel model)
    {
        await _service.NewItem(model);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostNewBrand(BrandModel model)
    {
        await _service.NewBrand(model);
        return Ok();
    }
}