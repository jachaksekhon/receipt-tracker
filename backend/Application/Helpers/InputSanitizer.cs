using ReceiptTracker.Application.Constants;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ReceiptTracker.Application.Helpers;

public class InputSanitizer
{
    // Normalizes and validates an email address
    public static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(ErrorMessages.InvalidEmail);

        email = email.Trim().ToLowerInvariant();

        if (!new EmailAddressAttribute().IsValid(email))
            throw new FormatException(ErrorMessages.InvalidEmailFormat);

        return email;
    }
}
