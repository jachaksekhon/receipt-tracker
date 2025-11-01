using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptTracker.Domain.Models.Receipts;

public class ReceiptItem
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Receipt")]
    public int ReceiptId { get; set; }

    [Required]
    public string ItemName { get; set; } = string.Empty;

    public string? ProductSku { get; set; }

    public int Quantity { get; set; } = 1;

    // Price fields
    [Column(TypeName = "decimal(10,2)")]
    public decimal OriginalPrice { get; set; }      

    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }    

    [Column(TypeName = "decimal(10,2)")]
    public decimal FinalPrice { get; set; }         

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? DepositFee { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? EnvironmentalFee { get; set; }

    public string? Category { get; set; }

    // For manual entry tracking
    public bool IsManuallyAdded { get; set; } = false;

    // ✅ Derived helper (not stored in DB)
    [NotMapped]
    public bool WasDiscounted => DiscountAmount > 0;
}
