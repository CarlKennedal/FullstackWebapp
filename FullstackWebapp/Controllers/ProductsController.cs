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

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{identifier}")] 
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(string identifier)
    {
        if (int.TryParse(identifier, out var id))
        {
            var productById = await _productService.GetProductByIdAsync(id);
            return productById != null ? Ok(productById) : NotFound();
        }

        var productByName = await _productService.GetProductByNameAsync(identifier);
        return productByName != null ? Ok(productByName) : NotFound();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteProductAsync(id);
        return success ? NoContent() : NotFound();
    }
}