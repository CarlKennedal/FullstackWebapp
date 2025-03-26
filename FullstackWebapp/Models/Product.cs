using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullstackWebapp.Models;
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string ProductNumber { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public string Category { get; set; }

    public bool IsDiscontinued { get; set; }
}