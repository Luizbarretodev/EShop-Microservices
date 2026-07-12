using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EShop.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public long Stock { get; set; }
    [Display(Name = "Image URL")]
    public string? ImageURL { get; set; }

    [Display(Name ="Categories")]
    public string? CategoryName { get; set; }
    public int Quantity { get; set; } = 1;

    [Display(Name = "Category")]
    public int? CategoryId { get; set; }
}
