namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptItemConfirmDto
{
    public string? ProductSku { get; set; }
    public required string ItemName { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal OriginalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalPrice { get; set; }
    public string? Category { get; set; }
}
