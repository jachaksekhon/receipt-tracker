using ReceiptTracker.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptTracker.Domain.Models.Receipts;

public class Receipt
{
    public enum ReceiptStatus
    {
        Uploaded = 1,
        Processing = 2,
        PendingReview = 3,
        Processed = 4,
        ErrorProcessing = 5
    }

    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public string StoreName { get; set; } = string.Empty;

    public string? ReceiptName { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public int TotalNumberOfItems { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public ReceiptStatus Status { get; set; }
    public string? Notes { get; set; }
    public User? User { get; set; }

    public List<ReceiptItem> Items { get; set; } = new();
}
