using ReceiptTracker.DTOs.ReceiptItems;

namespace ReceiptTracker.DTOs.Receipts;

public class ReceiptReadDto
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int ItemCount => Items?.Count ?? 0;
    public List<ReceiptItemReadDto> Items { get; set; } = new();
}
