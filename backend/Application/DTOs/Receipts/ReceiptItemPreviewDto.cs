namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptItemPreviewDto
{
    public string? ProductSku { get; init; }
    public string ItemName { get; init; } = "";
    public int Quantity { get; init; }
    public decimal OriginalPrice { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
    public string? Category { get; init; }
}
