using ReceiptTracker.Domain.Models.Receipts;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.Domain.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public List<Receipt> Receipts { get; set; } = new();
}