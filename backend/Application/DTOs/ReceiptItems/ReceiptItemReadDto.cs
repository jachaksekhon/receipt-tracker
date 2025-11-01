namespace ReceiptTracker.Application.DTOs.ReceiptItems;

public class ReceiptItemReadDto
{
    public int Id { get; set; }
    public string? ProductSku { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalPrice { get; set; }
    public string? Category { get; set; }
}
