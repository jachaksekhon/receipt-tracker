using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptTracker.Models.Receipts;

public class Receipt
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public string StoreName { get; set; } = string.Empty;

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public User? User { get; set; }

    public List<ReceiptItem> Items { get; set; } = new();
}
