using ReceiptTracker.Application.DTOs.ReceiptItems;
using ReceiptTracker.Domain.Models.Receipts;

namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptViewDto
{
    public int Id { get; set; }
    public string? ReceiptName { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalNumberOfItems => Items?.Count ?? 0;
    public string? Notes { get; set; }
    public List<ReceiptItemReadDto> Items { get; set; } = new();
}
