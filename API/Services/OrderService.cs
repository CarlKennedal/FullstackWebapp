using FullstackWebapp.Shared.Models;
using FullstackWebapp.API.Repositories;

public class OrderService
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IRepository<Customer> _customerRepo;
    private readonly IRepository<Product> _productRepo;

    public OrderService(
        IRepository<Order> orderRepo,
        IRepository<Customer> customerRepo,
        IRepository<Product> productRepo)
    {
        orderRepo = _orderRepo;
        customerRepo = _customerRepo;
        productRepo = _productRepo;
    }

    public async Task<(Order? order, string? error)> CreateOrder(Order order)
    {
        if (await _customerRepo.GetByIdAsync(order.CustomerId) == null)
            return (null, "Customer not found");

        foreach (var item in order.Items)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
                return (null, $"Product {item.ProductId} not found");
            if (product.IsDiscontinued)
                return (null, $"Product {product.Name} is discontinued");

            item.UnitPrice = product.Price;
        }

        order.OrderDate = DateTime.UtcNow;
        await _orderRepo.AddAsync(order);
        await _orderRepo.SaveChangesAsync();
        return (order, null);
    }

    public async Task<(bool success, string? error)> UpdateOrder(int orderId, Order updatedOrder)
    {
        var existingOrder = await _orderRepo.GetByIdAsync(orderId);
        if (existingOrder == null)
            return (false, "Order not found");

        existingOrder.CustomerId = updatedOrder.CustomerId;

        existingOrder.Items.Clear();
        foreach (var item in updatedOrder.Items)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
                return (false, $"Product {item.ProductId} not found");

            existingOrder.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        await _orderRepo.UpdateAsync(existingOrder);
        await _orderRepo.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null) return false;

        await _orderRepo.DeleteAsync(orderId);
        await _orderRepo.SaveChangesAsync();
        return true;
    }

    public async Task<List<Order>> SearchOrders(
     int? orderId = null,
     int? customerId = null,
     int? productId = null)
    {
        var orders = await _orderRepo.GetAllAsync();

        if (orderId.HasValue)
            orders = orders.Where(o => o.Id == orderId.Value).ToList();

        if (customerId.HasValue)
            orders = orders.Where(o => o.CustomerId == customerId.Value).ToList();

        if (productId.HasValue)
            orders = orders.Where(o => o.Items.Any(i => i.ProductId == productId.Value)).ToList();

        foreach (var order in orders)
        {
            order.Customer = await _customerRepo.GetByIdAsync(order.CustomerId);
        }

        return (List<Order>)orders;
    }
}