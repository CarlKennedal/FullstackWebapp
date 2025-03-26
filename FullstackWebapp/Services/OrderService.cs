using FullstackWebapp.Models;
using FullstackWebapp.Repositories;

public class OrderService(IRepository<Order> orderRepo)
{
    public async Task<IEnumerable<Order>> GetAllOrdersAsync() => await orderRepo.GetAllAsync();
    public async Task<Order?> GetOrderByIdAsync(int id) => await orderRepo.GetByIdAsync(id);
    public async Task AddOrderAsync(Order order) => await orderRepo.AddAsync(order);
    public async Task UpdateOrderAsync(Order order) => await orderRepo.UpdateAsync(order);
    public async Task DeleteOrderAsync(int id) => await orderRepo.DeleteAsync(id);

    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
    {
        var orders = await orderRepo.GetAllAsync();
        return orders.Where(o => o.CustomerId == customerId);
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int days)
    {
        var orders = await orderRepo.GetAllAsync();
        return orders.Where(o => o.OrderDate >= DateTime.Now.AddDays(-days));
    }
}