﻿using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.DTOs.Auth;

public class UserLoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
