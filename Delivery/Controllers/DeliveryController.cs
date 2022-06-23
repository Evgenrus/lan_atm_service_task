using Delivery.Data;
using Delivery.Data.Entities;
using Delivery.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class DeliveryController : ControllerBase
{
    private IDeliveryService _service { get; set; }

    public DeliveryController(IDeliveryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> PostNewDelivery(DeliveryInfo delivery)
    {
        try
        {
            await _service.PostNewDelivery(delivery);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AllFreeOrders()
    {
        try
        {
            var orders = await _service.GetAllFreeOrders();
            if (orders.Count <= 0)
            {
                return Ok("No free ");
            }

            return Ok(orders);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> BeginDelivery(int orderId)
    {
        try
        {
            await _service.BeginOrderDelivery(orderId, 1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveOrderFromStock(int orderId)
    {
        try
        {
            await _service.ReceiveOrderFromStock(orderId, 1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> FinishDelivery(int orderId)
    {
        try
        {
            await _service.FinishDelivery(orderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> DeliveryStatus(int orderId)
    {
        try
        {
            var order = await _service.DeliveryStatus(orderId);
            if (order is null)
            {
                return BadRequest("Couldn't find order");
            }

            return Ok(order);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCourier(CourierModel courier)
    {
        try
        {
            var res = await _service.RegisterCourier(
                courier.Name, 
                courier.Surname, 
                courier.Email, 
                courier.PhoneNumber);
            return Ok(res);
        } 
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}