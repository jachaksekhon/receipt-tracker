using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.DTOs.Auth;

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
