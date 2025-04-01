namespace FullstackWebapp.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(item => item.Quantity * item.UnitPrice);
}

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}