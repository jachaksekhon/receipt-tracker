using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using ReceiptTracker.Application.Constants;
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

        var parsedDto = await parser.ParseAsync(stream);

        Console.WriteLine(parsedDto.Items);

        existing.StoreName = parsedDto.StoreName;
        existing.PurchaseDate = DateTime.SpecifyKind(parsedDto.PurchaseDate, DateTimeKind.Utc);
        existing.TotalAmount = parsedDto.TotalAmount;
        existing.TotalNumberOfItems = parsedDto.TotalNumberOfItems;
        existing.Status = Receipt.ReceiptStatus.Processed;

        existing.Items = new List<ReceiptItem>();


        foreach (var itemDto in parsedDto.Items)
        {
            existing.Items.Add(new ReceiptItem
            {
                ItemName = itemDto.ItemName,
                ProductSku = itemDto.ProductSku,
                Quantity = itemDto.Quantity,
                OriginalPrice = itemDto.OriginalPrice,                    
                DiscountAmount = itemDto.DiscountAmount,  
                FinalPrice = itemDto.FinalPrice,         
                Category = itemDto.Category
            });
        }

        await _receiptRepository.UpdateAsync(existing);

        // map receipt to read DTO
        return _mapper.Map<ReceiptReadDto>(existing);

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
