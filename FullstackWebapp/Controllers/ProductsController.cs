using Microsoft.AspNetCore.Mvc;
using FullstackWebapp.Models;
using FullstackWebapp.Repositories;
using FullstackWebapp.Services;
using Microsoft.EntityFrameworkCore;

namespace FullstackWebapp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    // Inject ProductService via constructor
    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}