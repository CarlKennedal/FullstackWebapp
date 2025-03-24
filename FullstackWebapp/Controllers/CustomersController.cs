using FullstackWebapp.Models;
using FullstackWebapp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FullstackWebapp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer> _repository;

    public CustomersController(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _repository.GetAllAsync();
        return Ok(customers);
    }

    // GET: api/customers/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        await _repository.AddAsync(customer);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    // PUT: api/customers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        await _repository.UpdateAsync(customer);
        return NoContent();
    }

    // DELETE: api/customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}