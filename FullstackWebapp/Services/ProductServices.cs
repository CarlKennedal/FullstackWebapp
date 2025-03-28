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

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
        => await _context.Products.FindAsync(id);

    public async Task<Product?> GetProductByNameAsync(string name)
        => await _context.Products
            .FirstOrDefaultAsync(p => p.Name == name);

    public async Task<(bool success, string? error)> AddProductAsync(Product product)
    {
        if (await _context.Products.AnyAsync(p => p.Name == product.Name))
        {
            return (false, "Product name already exists");
        }

        if (product.Id != 0 && await _context.Products.AnyAsync(p => p.Id == product.Id))
        {
            return (false, "Product ID already exists");
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return (true, null);
    }
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}