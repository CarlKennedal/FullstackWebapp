using FullstackWebapp.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace FullstackWebapp.GUI.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public ApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config["ApiBaseUrl"] ?? "https://localhost:7178;http://localhost:5166";
    }

    // ===== CUSTOMERS =====
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _http.GetFromJsonAsync<List<Customer>>($"{_baseUrl}/api/customers")
               ?? new List<Customer>();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Customer>($"{_baseUrl}/api/customers/{id}");
    }

    public async Task<Customer?> CreateCustomerAsync(Customer customer)
    {
        var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/customers", customer);
        return await response.Content.ReadFromJsonAsync<Customer>();
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/api/customers/{customer.Id}", customer);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var response = await _http.DeleteAsync($"{_baseUrl}/api/customers/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Customer>> SearchCustomersAsync(int? id, string? firstName, string? lastName, string? email, string? phone)
    {
        var queryParams = new List<string>();
        if (id.HasValue) queryParams.Add($"id={id}");
        if (!string.IsNullOrEmpty(firstName)) queryParams.Add($"firstName={firstName}");
        if (!string.IsNullOrEmpty(lastName)) queryParams.Add($"lastName={lastName}");
        if (!string.IsNullOrEmpty(email)) queryParams.Add($"email={email}");
        if (!string.IsNullOrEmpty(phone)) queryParams.Add($"phone={phone}");

        var queryString = queryParams.Any() ? $"?{string.Join("&", queryParams)}" : "";
        return await _http.GetFromJsonAsync<List<Customer>>($"{_baseUrl}/api/customers/search{queryString}")
               ?? new List<Customer>();
    }

    // ===== PRODUCTS =====
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _http.GetFromJsonAsync<List<Product>>($"{_baseUrl}/api/products")
               ?? new List<Product>();
    }

    public async Task<Product?> GetProductAsync(string identifier)
    {
        return await _http.GetFromJsonAsync<Product>($"{_baseUrl}/api/products/{identifier}");
    }

    public async Task<Product?> CreateProductAsync(Product product)
    {
        var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/products", product);
        return await response.Content.ReadFromJsonAsync<Product>();
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/api/products/{product.Id}", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _http.DeleteAsync($"{_baseUrl}/api/products/{id}");
        return response.IsSuccessStatusCode;
    }

    // ===== ORDERS =====
    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _http.GetFromJsonAsync<List<Order>>($"{_baseUrl}/api/orders")
               ?? new List<Order>();
    }

    public async Task<Order?> CreateOrderAsync(Order order)
    {
        var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/orders", order);
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        var orders = await _http.GetFromJsonAsync<List<Order>>($"{_baseUrl}/api/orders/search?orderId={id}");
        return orders?.FirstOrDefault();
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/api/orders/{order.Id}", order);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var response = await _http.DeleteAsync($"{_baseUrl}/api/orders/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Order>> SearchOrdersAsync(int? orderId, int? customerId, int? productId)
    {
        var queryParams = new List<string>();
        if (orderId.HasValue) queryParams.Add($"orderId={orderId}");
        if (customerId.HasValue) queryParams.Add($"customerId={customerId}");
        if (productId.HasValue) queryParams.Add($"productId={productId}");

        var queryString = queryParams.Any() ? $"?{string.Join("&", queryParams)}" : "";
        return await _http.GetFromJsonAsync<List<Order>>($"{_baseUrl}/api/orders/search{queryString}")
               ?? new List<Order>();
    }
}