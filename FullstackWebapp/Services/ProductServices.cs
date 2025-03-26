using FullstackWebapp.Models;
using FullstackWebapp.Data;
using Microsoft.EntityFrameworkCore;

namespace FullstackWebapp.Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
        => await _context.Products.ToListAsync();

    public async Task<Product?> GetProductByIdAsync(int id)
        => await _context.Products.FindAsync(id);

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Created product with ID: {product.Id}");
    }
}