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
    public async Task<IActionResult> AddItemToCart(int productid, int count = 1)
    {
        
    }
}