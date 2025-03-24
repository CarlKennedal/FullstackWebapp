namespace FullstackWebapp.Models;
public class Product
{
    public int Id { get; set; }
    public required string ProductNumber { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string Category { get; set; } // e.g., "Electronics"
    public bool IsDiscontinued { get; set; }
}