using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.Application.DTOs.Receipts;

public class ReceiptUploadDto
{
    [Required]
    public IFormFile File { get; set; } = default!;
    public string? Notes { get; set; }
}
