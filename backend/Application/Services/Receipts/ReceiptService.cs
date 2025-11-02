using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using ReceiptTracker.Application.Constants;
using ReceiptTracker.Application.DTOs.ReceiptItems;
using ReceiptTracker.Application.DTOs.Receipts;
using ReceiptTracker.Domain.Models.Receipts;
using ReceiptTracker.Infrastructure.FileStorage;
using ReceiptTracker.Infrastructure.Parsers;
using ReceiptTracker.Infrastructure.Repositories.Receipts;

namespace ReceiptTracker.Application.Services.Receipts;

public class ReceiptService : IReceiptService
{
    private const int MaxUploadSizeMB = 10;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png", "application/pdf" };

    private readonly IReceiptRepository _receiptRepository;
    private readonly IEnumerable<IReceiptParser> _parsers;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public ReceiptService(
        IReceiptRepository receiptRepository, 
        IEnumerable<IReceiptParser> parsers,
        IWebHostEnvironment env,
        IMapper mapper)
    {
        _receiptRepository = receiptRepository;
        _parsers = parsers;
        _env = env;
        _mapper = mapper;
    }

    public async Task<ReceiptReadDto> UploadReceiptAsync(ReceiptUploadDto uploadDto, int userId)
    {
        var imageUrl = await SaveImageAsync(uploadDto.File);

        var receipt = new Receipt
        {
            UserId = userId,
            ImageUrl = imageUrl,
            StoreName = "Unknown",
            PurchaseDate = DateTime.UtcNow,
            TotalAmount = 0,
            Notes = uploadDto.Notes,
            Status = Receipt.ReceiptStatus.Uploaded,
        };

        try
        {
            await _receiptRepository.CreateAsync(receipt);
        }
        catch (Exception ex)
        {
            var fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            throw new Exception("Database operation failed when creating receipt.", ex);
        }

        return _mapper.Map<ReceiptReadDto>(receipt);
    }

    public async Task<ReceiptReadDto> ProcessReceiptAsync(int receiptId, int userId)
    {
        var existing = await _receiptRepository.FindByIdAsync(receiptId, userId);
        if (existing == null)
            throw new Exception($"Receipt with ID {receiptId} not found for this user.");

        // Get full image file path
        var fullImagePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
        Console.WriteLine(fullImagePath);
        if (!File.Exists(fullImagePath))
            throw new Exception("Receipt image file not found on server.");

        // Open file stream for the image
        await using var stream = File.OpenRead(fullImagePath);

        var parser = _parsers.FirstOrDefault(p => p.CanParse("")); 
        if (parser == null)
            throw new Exception("No suitable parser available.");


        existing.Status = Receipt.ReceiptStatus.Processing;

        var parsedDto = await parser.ParseAsync(stream);

        existing.StoreName = parsedDto.StoreName;
        existing.PurchaseDate = DateTime.SpecifyKind(parsedDto.PurchaseDate, DateTimeKind.Utc);
        existing.TotalAmount = parsedDto.TotalAmount;
        existing.TotalNumberOfItems = parsedDto.TotalNumberOfItems;
        existing.Status = Receipt.ReceiptStatus.PendingReview;

        existing.Items = new List<ReceiptItem>();

        existing.Items = parsedDto.Items.Select(i => new ReceiptItem
        {
            ItemName = i.ItemName,
            ProductSku = i.ProductSku,
            Quantity = i.Quantity,
            OriginalPrice = i.OriginalPrice,
            DiscountAmount = i.DiscountAmount,
            FinalPrice = i.FinalPrice,
            Category = i.Category
        }).ToList();

        await _receiptRepository.UpdateAsync(existing);

        var previewDto = _mapper.Map<ReceiptReadDto>(existing);

        return previewDto;
    }

    public async Task<ReceiptReadDto> GetReceiptPreviewAsync(int receiptId, int userId)
    {
        var existing = await _receiptRepository.FindByIdAsync(receiptId, userId)
            ?? throw new Exception($"Receipt with ID {receiptId} not found for this user.");

        if (existing.Status is not (Receipt.ReceiptStatus.PendingReview
                             or Receipt.ReceiptStatus.Processing
                             or Receipt.ReceiptStatus.Processed))
        {
            throw new Exception("Receipt is not ready for preview.");
        }

        // Just map the stored DB entity to the same DTO the frontend expects
        var previewDto = _mapper.Map<ReceiptReadDto>(existing);
        return previewDto;
    }

    public async Task<ReceiptReadDto> ConfirmReceiptAsync(int receiptId, int userId, ReceiptConfirmDto receiptConfirmDto)
    {
        var existing = await _receiptRepository.FindByIdAsync(receiptId, userId)
        ?? throw new Exception($"Receipt with ID {receiptId} not found for this user.");

        // 2️⃣ Update main receipt fields with the user's edited data
        existing.StoreName = receiptConfirmDto.StoreName;
        existing.PurchaseDate = DateTime.SpecifyKind(receiptConfirmDto.PurchaseDate, DateTimeKind.Utc);
        existing.TotalAmount = receiptConfirmDto.TotalAmount;
        existing.TotalNumberOfItems = receiptConfirmDto.Items.Count;

        // You can safely keep this — it won't break future edits
        existing.Status = Receipt.ReceiptStatus.Processed;

        // 3️⃣ Clear and rebuild items from user-provided DTO
        existing.Items.Clear();
        foreach (var item in receiptConfirmDto.Items)
        {
            existing.Items.Add(new ReceiptItem
            {
                ProductSku = item.ProductSku,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                OriginalPrice = item.OriginalPrice,
                DiscountAmount = item.DiscountAmount,
                FinalPrice = item.FinalPrice,
                Category = item.Category
            });
        }

        // Again needs to be updated, this is not accurate since we dont include depoists / fees
        existing.TotalAmount = existing.Items.Sum(i => i.FinalPrice * i.Quantity);

        await _receiptRepository.UpdateAsync(existing);

        var resultDto = _mapper.Map<ReceiptReadDto>(existing);
        return resultDto;
    }
    public async Task<IReadOnlyList<ReceiptReadDto>> GetAllReceiptsForUserAsync(int userId)
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
