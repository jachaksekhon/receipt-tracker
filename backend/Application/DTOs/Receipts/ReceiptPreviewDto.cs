using ReceiptTracker.Application.DTOs.ReceiptItems;

namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptPreviewDto
{
    public int Id { get; init; }  
    public string StoreName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalNumberOfItems { get; set; }
    public List<ReceiptItemPreviewDto> Items { get; set; } = new();
}

