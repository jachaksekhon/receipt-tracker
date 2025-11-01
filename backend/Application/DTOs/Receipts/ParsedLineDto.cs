namespace ReceiptTracker.Application.DTOs.Receipts;

public class ParsedLineDto
{
    public string RawText { get; set; } = string.Empty;
    public string? Sku { get; set; }
    public decimal Price { get; set; }
    public bool IsNegative { get; set; }
}
