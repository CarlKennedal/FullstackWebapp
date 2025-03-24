using Microsoft.AspNetCore.Mvc;
using FullstackWebapp.Models;
using FullstackWebapp.Repositories;

namespace FullstackWebapp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IRepository<Product> repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await repository.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await repository.AddAsync(product);
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
    }
}