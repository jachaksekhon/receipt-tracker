namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptConfirmDto
{
    public required string StoreName { get; set; }
    public required DateTime PurchaseDate { get; set; }
    public required decimal TotalAmount { get; set; }
    public required List<ReceiptItemConfirmDto> Items { get; set; } = new();
}
