using FullstackWebapp.Data;
using FullstackWebapp.Models;
using FullstackWebapp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullstackWebapp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        var (createdOrder, error) = await _orderService.CreateOrder(order);
        if (error != null) return BadRequest(error);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var orders = await _orderService.SearchOrders(orderId: id);
        return orders.Count > 0 ? Ok(orders[0]) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        if (id != order.Id) return BadRequest("ID mismatch");

        var (success, error) = await _orderService.UpdateOrder(id, order);
        if (!success) return BadRequest(error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _orderService.DeleteOrder(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] int? orderId,
        [FromQuery] int? customerId,
        [FromQuery] int? productId)
    {
        var orders = await _orderService.SearchOrders(orderId, customerId, productId);
        return Ok(orders);
    }
}