﻿using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.Application.DTOs.Users;

public class UserPasswordUpdateDto
{
    [Required(ErrorMessage = "Current password is required.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string NewPassword { get; set; } = string.Empty;
}

