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
    public decimal Price { get; set; }

    public bool IsDiscount {  get; set; }
    public string? Category { get; set; }

}
