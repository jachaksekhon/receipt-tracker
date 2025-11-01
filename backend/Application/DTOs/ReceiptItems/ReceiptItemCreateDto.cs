using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.Application.DTOs.ReceiptItems;

public class ReceiptItemCreateDto
{
    [Required]
    public string ItemName { get; set; } = string.Empty;

    public string? ProductSku {  get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
    public decimal OriginalPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount amount cannot be negative")]
    public decimal DiscountAmount {  get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
    public decimal FinalPrice { get; set; }
    public string? Category { get; set; }
}
