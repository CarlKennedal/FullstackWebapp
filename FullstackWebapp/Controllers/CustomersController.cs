using FullstackWebapp.Models;
using FullstackWebapp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FullstackWebapp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        try
        {
            var createdCustomer = await _customerService.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        await _customerService.UpdateCustomerAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _customerService.DeleteCustomerAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomers(
     [FromQuery] int? id,
     [FromQuery] string? firstName,
     [FromQuery] string? lastName,
     [FromQuery] string? email,
     [FromQuery] string? phone)
    {
        if (id == null &&
            string.IsNullOrEmpty(firstName) &&
            string.IsNullOrEmpty(lastName) &&
            string.IsNullOrEmpty(email) &&
            string.IsNullOrEmpty(phone))
        {
            return BadRequest("Please provide at least one search parameter");
        }

        var customers = await _customerService.SearchCustomersAsync(
            id: id,
            firstName: firstName,
            lastName: lastName,
            email: email,
            phone: phone);

        return Ok(customers);
    }
}