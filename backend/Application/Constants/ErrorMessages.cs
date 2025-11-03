namespace ReceiptTracker.Application.Constants;

public static class ErrorMessages
{
    // Common Validation Errors
    public const string InvalidId = "Id must be a positive integer.";
    public const string InvalidEmail = "Email cannot be empty.";
    public const string InvalidEmailFormat = "Invalid email format.";
    public const string InvalidFile = "Invalid or missing file.";
    public const string InvalidToken = "Invalid or expired reset token";
    public const string InvalidPassword = "Password does not match our records.";

    // User Errors
    public const string FailedToDeleteUser = "Failed to delete user.";
    public const string UserAlreadyExists = "A user with this email already exists.";
    public static string UserNotFoundbyId(int id) => $"No user found with the specified ID '{id}'.";
    public static string UserNotFoundByEmail(string email) => $"No user found with the specified email '{email}'.";

    // Receipt Errors

    public const string ReceiptUploadFailed = "Failed to upload receipt image.";
    public const string ReceiptProcessFailed = "Failed to process receipt.";
    public const string InvalidReceiptFormat = "Invalid receipt format or unsupported file type.";
    public const string ReceiptImageFileNotFound = "Receipt image file not found.";

    public const string ParserNotAvailable = "No suitable parser found for this receipt type";

    public const string ReceiptNotReadyForPreview = "Receipt is not ready for preview.";
    public const string FailedToUploadFileToServer = "Error uploading image to server. Please try again later.";
    public const string FailedToSaveFileToDatabase = "Error saving receipt to database. Please try again later.";
    public const string FailedToProcessReceipt = "An unexpected error occurred while processing the receipt.";
    public const string UploadCostcoReceipt = "Only Costco receipts are supported. Please upload a valid Costco receipt.";
    public static string ReceiptNotFound(int id) => $"Receipt with ID '{id}' not found.";
    public static string ReceiptImageNotFound(int id) => $"Could not retrieve image for receipt with ID '{id}'.";


    // System / Generic
    public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
    public const string ErrorConfirmingReceipt = "An unexpected error occurred while confirming the receipt.";
}
