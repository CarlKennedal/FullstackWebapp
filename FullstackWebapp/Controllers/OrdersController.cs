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
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly AppDbContext _context;

    public OrdersController(
        IRepository<Order> orderRepository,
        IRepository<Customer> customerRepository,
        IRepository<Product> productRepository,
        AppDbContext context)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order)
    {
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == order.CustomerId);
        if (!customerExists) return BadRequest("Customer not found");

        foreach (var item in order.Items)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductId);
            if (!productExists) return BadRequest($"Product {item.ProductId} not found");
        }

        await _orderRepository.AddAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderRepository.DeleteAsync(id);
        return NoContent();
    }
}