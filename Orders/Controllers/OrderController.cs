using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Orders.Database;
using Orders.Database.Entities;
using Orders.Service;

namespace Orders.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private OrderDbContext _context { get; set; }
    private IOrderService _service { get; set; }

    public OrderController(OrderDbContext context, IOrderService orderService)
    {
        _context = context;
        _service = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> PostItemToCart(Item item, int cartId, int count = 1)
    {
        if (count < 1)
            return BadRequest("Count should be equal or greater then 1");
        if (cartId < 1)
            return BadRequest("Wrong ID");
        try
        {
            await _service.AddItemToCart(cartId, item, count);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> CartItemCountChange(int cartId, int cartItemId, int countChange)
    {
        if (cartId < 1 || cartItemId < 1)
        {
            return BadRequest("Wrong Id");
        }

        try
        {
            await _service.ChangeItemCountInCart(cartId, cartItemId, countChange);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(int cartId)
    {
        if (cartId < 1)
        {
            return BadRequest("Wrong cartId");
        }

        try
        {
            await _service.CreateOrder(cartId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        if (orderId < 1)
        {
            return BadRequest("Wrong cartId");
        }

        try
        {
            await _service.CancelOrder(orderId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> FinishOrder(int orderId)
    {
        if (orderId < 1)
        {
            return BadRequest("Wrong cartId");
        }

        try
        {
            await _service.FinishOrder(orderId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
    
}