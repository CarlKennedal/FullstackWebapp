using FullstackWebapp.Models;
using FullstackWebapp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FullstackWebapp.Repositories;
namespace FullstackWebapp.Services;

public class ProductService
{
    private readonly IRepository<Product> _productRepo;

    public ProductService(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var products = await _productRepo.GetAllAsync();
        return products.ToList();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return (await _productRepo.GetAllAsync())
            .FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return (await _productRepo.GetAllAsync())
            .FirstOrDefault(p => p.Name == name);
    }

    public async Task<(bool success, string? error)> AddProductAsync(Product product)
    {
        var allProducts = await _productRepo.GetAllAsync();

        if (allProducts.Any(p => p.Name == product.Name))
            return (false, "Product name already exists");

        if (product.Id != 0 && allProducts.Any(p => p.Id == product.Id))
            return (false, "Product ID already exists");

        await _productRepo.AddAsync(product);
        await _productRepo.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool success, string? error)> UpdateProductAsync(Product product)
    {
        var allProducts = await _productRepo.GetAllAsync();
        var existing = allProducts.FirstOrDefault(p => p.Id == product.Id);

        if (existing == null)
            return (false, "Product not found");

        if (allProducts.Any(p => p.Name == product.Name && p.Id != product.Id))
            return (false, "Product name already exists");

        if (product.Price <= 0)
            return (false, "Price must be greater than 0");

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.IsDiscontinued = product.IsDiscontinued;
        existing.ProductNumber = product.ProductNumber;
        existing.Category = product.Category;

        await _productRepo.UpdateAsync(existing);
        await _productRepo.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var allProducts = await _productRepo.GetAllAsync();
        var product = allProducts.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return false;

        await _productRepo.DeleteAsync(id);
        await _productRepo.SaveChangesAsync();
        return true;
    }
}