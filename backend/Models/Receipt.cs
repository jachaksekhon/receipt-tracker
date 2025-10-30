using Microsoft.AspNetCore.Http.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptTracker.Models;

public class Receipt
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Required]
    public string StoreName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    public string ImageUrl { get; set; } = string.Empty;
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }
    public int ItemCount => Items.Count;

    public List<ReceiptItem> Items = new();
    public User? User { get; set; }

}
