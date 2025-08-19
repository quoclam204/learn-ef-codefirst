using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStoreMVC.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = "";

    [Range(0, 100000000)]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Giá")]
    public decimal Price { get; set; }

    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
