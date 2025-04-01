using FullstackWebapp.Models;
using FullstackWebapp.Repositories;

public class OrderService
{
    private readonly IRepository<Order> orderRepo;
    private readonly IRepository<Customer> customerRepo;
    private readonly IRepository<Product> productRepo;

    public OrderService(
        IRepository<Order> orderRepo,
        IRepository<Customer> customerRepo,
        IRepository<Product> productRepo)
    {
        orderRepo = orderRepo;
        customerRepo = customerRepo;
        productRepo = productRepo;
    }

    public async Task<(Order? order, string? error)> CreateOrder(Order order)
    {
        if (await customerRepo.GetByIdAsync(order.CustomerId) == null)
            return (null, "Customer not found");

        foreach (var item in order.Items)
        {
            var product = await productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
                return (null, $"Product {item.ProductId} not found");
            if (product.IsDiscontinued)
                return (null, $"Product {product.Name} is discontinued");

            item.UnitPrice = product.Price;
        }

        order.OrderDate = DateTime.UtcNow;
        await orderRepo.AddAsync(order);
        await orderRepo.SaveChangesAsync();
        return (order, null);
    }

    public async Task<(bool success, string? error)> UpdateOrder(int orderId, Order updatedOrder)
    {
        var existingOrder = await orderRepo.GetByIdAsync(orderId);
        if (existingOrder == null)
            return (false, "Order not found");

        existingOrder.CustomerId = updatedOrder.CustomerId;

        existingOrder.Items.Clear();
        foreach (var item in updatedOrder.Items)
        {
            var product = await productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
                return (false, $"Product {item.ProductId} not found");

            existingOrder.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        await orderRepo.UpdateAsync(existingOrder);
        await orderRepo.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        var order = await orderRepo.GetByIdAsync(orderId);
        if (order == null) return false;

        await orderRepo.DeleteAsync(orderId);
        await orderRepo.SaveChangesAsync();
        return true;
    }

    public async Task<List<Order>> SearchOrders(
     int? orderId = null,
     int? customerId = null,
     int? productId = null)
    {
        var orders = await orderRepo.GetAllAsync();

        if (orderId.HasValue)
            orders = orders.Where(o => o.Id == orderId.Value).ToList();

        if (customerId.HasValue)
            orders = orders.Where(o => o.CustomerId == customerId.Value).ToList();

        if (productId.HasValue)
            orders = orders.Where(o => o.Items.Any(i => i.ProductId == productId.Value)).ToList();

        foreach (var order in orders)
        {
            order.Customer = await customerRepo.GetByIdAsync(order.CustomerId);
        }

        return (List<Order>)orders;
    }
}