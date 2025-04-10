using FullstackWebapp.Shared.Models;
using System.Net.Http.Json;
using System.Net;

namespace FullstackWebapp.GUI.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient http, ILogger<ApiService> logger)
    {
        _http = http;
        _logger = logger;
    }

    // ===== CUSTOMERS =====
    public async Task<List<Customer>> GetCustomersAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Customer>>("api/customers") ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customers");
            return new();
        }
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<Customer>($"api/customers/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting customer {id}");
            return null;
        }
    }

    public async Task<(bool success, Customer? customer, string? error)> CreateCustomerAsync(Customer customer)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/customers", customer);
            if (response.IsSuccessStatusCode)
            {
                return (true, await response.Content.ReadFromJsonAsync<Customer>(), null);
            }
            return (false, null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return (false, null, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> UpdateCustomerAsync(Customer customer)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/customers/{customer.Id}", customer);
            return (response.IsSuccessStatusCode,
                   response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating customer {customer.Id}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> DeleteCustomerAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/customers/{id}");
            return (response.IsSuccessStatusCode,
                   response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting customer {id}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> SaveCustomerAsync(Customer customer)
    {
        if (customer.Id == 0)
        {
            var createResult = await CreateCustomerAsync(customer);
            return (createResult.success, createResult.error);
        }
        else
        {
            var updateResult = await UpdateCustomerAsync(customer);
            return updateResult;
        }
    }

    // ===== PRODUCTS =====
    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Product>>("api/products") ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products");
            return new();
        }
    }

    public async Task<Product?> GetProductAsync(string identifier)
    {
        try
        {
            return await _http.GetFromJsonAsync<Product>($"api/products/{identifier}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting product {identifier}");
            return null;
        }
    }

    public async Task<(bool success, Product? product, string? error)> CreateProductAsync(Product product)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/products", product);
            if (response.IsSuccessStatusCode)
            {
                return (true, await response.Content.ReadFromJsonAsync<Product>(), null);
            }
            return (false, null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return (false, null, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> UpdateProductAsync(Product product)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/products/{product.Id}", product);
            return (response.IsSuccessStatusCode,
                   response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating product {product.Id}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> DeleteProductAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/products/{id}");
            return (response.IsSuccessStatusCode,
                   response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product {id}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> SaveProductAsync(Product product)
    {
        if (product.Id == 0)
        {
            var createResult = await CreateProductAsync(product);
            return (createResult.success, createResult.error);
        }
        else
        {
            var updateResult = await UpdateProductAsync(product);
            return updateResult;
        }
    }

    // ===== ORDERS =====
    public async Task<List<Order>> GetOrdersAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Order>>("api/orders") ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders");
            return new();
        }
    }

    public async Task<(bool success, Order? order, string? error)> CreateOrderAsync(Order order)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/orders", order);
            if (response.IsSuccessStatusCode)
            {
                return (true, await response.Content.ReadFromJsonAsync<Order>(), null);
            }
            return (false, null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return (false, null, ex.Message);
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        try
        {
            var orders = await _http.GetFromJsonAsync<List<Order>>($"api/orders/search?orderId={id}");
            return orders?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order {id}");
            return null;
        }
    }

    public async Task<(bool success, string? error)> UpdateOrderAsync(Order order)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/orders/{order.Id}", order);
            return (response.IsSuccessStatusCode,
                   response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order {order.Id}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool success, string? error)> DeleteOrderAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/orders/{id}");
            return (response.IsSuccessStatusCode,
                   response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order {id}");
            return (false, ex.Message);
        }
    }
}