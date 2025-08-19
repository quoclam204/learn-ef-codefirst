using System.ComponentModel.DataAnnotations;

namespace ProductStoreMVC.Models;

public class Category
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = "";

    // Navigation
    public ICollection<Product>? Products { get; set; }
}
