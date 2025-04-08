using System.ComponentModel.DataAnnotations.Schema;

namespace FullstackWebapp.Shared.Models;

public class Customer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string MobilePhone { get; set; }
    public required string Address { get; set; }
}