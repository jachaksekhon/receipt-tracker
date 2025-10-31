using Microsoft.AspNetCore.Hosting;
using ReceiptTracker.DTOs.Receipts;
using ReceiptTracker.Helpers;
using ReceiptTracker.Models.Receipts;
using ReceiptTracker.Repositories.Receipts;
using ReceiptTracker.Services.Parsers;

namespace ReceiptTracker.Services.Receipts;

public class ReceiptService : IReceiptService
{
    private const int MaxUploadSizeMB = 10;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png", "application/pdf" };

    private readonly IReceiptRepository _receiptRepository;
    private readonly IEnumerable<IReceiptParser> _parsers;
    private readonly IWebHostEnvironment _env;

    public ReceiptService(
        IReceiptRepository receiptRepository, 
        IEnumerable<IReceiptParser> parsers,
        IWebHostEnvironment env)
    {
        _receiptRepository = receiptRepository;
        _parsers = parsers;
        _env = env;
    }
    public async Task<ReceiptReadDto> ProcessReceiptAsync(ReceiptUploadDto uploadDto, int userId)
    {
        if (uploadDto.File == null)
            throw new Exception("No file provided.");

        var imageUrl = await SaveImageAsync(uploadDto.File);

        throw new NotImplementedException();

    }
    public async Task<IReadOnlyList<Receipt>> GetAllReceiptsForUserAsync(int userId)
    {
        throw new NotImplementedException();
    }
    public async Task<ReceiptReadDto> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        throw new NotImplementedException();
    }

    // Helpers

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var safeFileName = FileUploadHelper.ValidateAndSanitizeFile(
            file,
            FileUploadHelper.ToBytes(MaxUploadSizeMB),
            AllowedExtensions,
            AllowedContentTypes
        );

        var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var filePath = Path.Combine(uploadsDir, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{uniqueFileName}";

    }

}
