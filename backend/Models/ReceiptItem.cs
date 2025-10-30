using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptTracker.Models;

public class ReceiptItem
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Receipt")]
    public int ReceiptId { get; set; }
    [Required]
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }
    public string? Category { get; set; }
    public string? Receipt { get; set; }
}
