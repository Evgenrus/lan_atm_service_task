using Microsoft.AspNetCore.Mvc;
using Orders.Database;

namespace Orders.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : Controller
{
    private OrderDbContext _context { get; set; }

    public OrderController(OrderDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToCart(int productid, int cartId, int count = 1)
    {
        if (count < 1)
            return BadRequest("Count should be equal or greater then 1");
        if (productid < 1 || cartId < 1)
            return BadRequest("Wrong ID");
    }
}