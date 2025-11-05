namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptDashboardDto
{
    public int Id { get; set; }
    public string? ReceiptName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal TotalSaved { get; set; } = 0;
    public int TotalNumberOfItems { get; set; }
    public int OnSaleItems { get; set; } = 0;
    public DateTime CreatedAt { get; set; }

}
