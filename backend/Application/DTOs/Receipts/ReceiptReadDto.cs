using ReceiptTracker.Application.DTOs.ReceiptItems;
using ReceiptTracker.Domain.Models.Receipts;

namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptReadDto
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount => Items?.Count ?? 0;
    public int TotalNumberOfItems { get; set; }
    public Receipt.ReceiptStatus Status { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<ReceiptItemReadDto> Items { get; set; } = new();
}
