using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.DTOs.Receipts;

public class ReceiptUploadDto
{
    [Required]
    public IFormFile File { get; set; } = default!;
    public string? Notes { get; set; }
}
