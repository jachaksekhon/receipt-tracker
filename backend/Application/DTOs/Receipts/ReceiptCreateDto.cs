using ReceiptTracker.Application.DTOs.ReceiptItems;
using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptCreateDto
{
    [Required]
    public string StoreName { get; set; } = string.Empty;
    public string? ReceiptName { get; set; }

    [Required]
    public DateTime PurchaseDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
    public decimal TotalAmount { get; set; }

    public int TotalNumberOfItems { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<ReceiptItemCreateDto> Items { get; set; } = new();
}
