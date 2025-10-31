using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.Application.DTOs.ReceiptItems;

public class ManualReceiptItemDto
{
    [Required]
    public int ReceiptId { get; set; }

    public string? ProductSku { get; set; }

    [Required]
    public string ItemName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public string? Category { get; set; }
}
