namespace ReceiptTracker.Infrastructure.FileStorage;

public static class FileUploadHelper
{
    public static string ValidateAndSanitizeFile(IFormFile file, long maxBytes, string[] allowedExtensions, string[] allowedContentTypes)
    {
        if (file == null || file.Length == 0)
            throw new Exception("No file uploaded or file is empty.");

        if (file.Length > maxBytes)
            throw new Exception($"File size exceeds {maxBytes / (1024 * 1024)} MB.");

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext.ToLower()))
            throw new Exception("Unsupported file type.");

        if (!allowedContentTypes.Contains(file.ContentType))
            throw new Exception("Unsupported content type.");

        var safeFileName = Path.GetFileNameWithoutExtension(file.FileName);
        safeFileName = string.Join("_", safeFileName.Split(Path.GetInvalidFileNameChars()));

        return $"{safeFileName}{ext.ToLower()}";
    }
    public static long ToBytes(int megabytes) => megabytes * 1024L * 1024L;
}
