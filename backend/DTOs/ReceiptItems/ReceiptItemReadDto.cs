namespace ReceiptTracker.DTOs.ReceiptItems;

public class ReceiptItemReadDto
{
    public int Id { get; set; }
    public string? ProductSku { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice => Quantity * Price;
    public string? Category { get; set; }
}
