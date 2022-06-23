using Catalog.Database;
using Catalog.Service;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CatalogController : ControllerBase
{
    private CatalogDbContext Context { get; set; }
    private ICatalogService Service { get; set; }

    public CatalogController(CatalogDbContext context, ICatalogService service)
    {
        Context = context;
        Service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CheckItem([FromBody]Item item)
    {
        try
        {
            var res = await Service.CheckItem(item);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CheckItems([FromBody]List<Item> items)
    {
        var VerifiedItems = new List<Item>();
        foreach(var item in items)
        {
            try
            {
                var res = await Service.CheckItem(item);
                VerifiedItems.Add(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        return Ok(VerifiedItems);
    }

    [HttpGet]
    public async Task<IActionResult> GetItemById(int id)
    {
        try
        {
            var prods = await Service.ItemById(id);
            return Ok(prods);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetItemByName(string name)
    {
        try
        {
            var prods = await Service.ItemByName(name);
            return Ok(prods);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetItemsByBrandName(string name)
    {
        try
        {
            var prods = await Service.ItemsByBrandName(name);
            return Ok(prods);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostNewItem(Item model)
    {
        try
        {
            await Service.NewItem(model);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostNewBrand(BrandModel model)
    {
        try
        {
            await Service.NewBrand(model);
            return Ok();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostNewCategory(CategoryModel model)
    {
        try
        {
            await Service.NewCategory(model);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}