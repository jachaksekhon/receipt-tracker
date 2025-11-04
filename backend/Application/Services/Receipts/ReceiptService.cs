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
        var imageUrl = string.Empty;

        try
        {
            imageUrl = await SaveImageAsync(uploadDto.File);
        }
        catch (ArgumentException ex)
        {
            throw new Exception(ErrorMessages.InvalidReceiptFormat, ex);
        }
        catch (IOException ex)
        {
            throw new Exception(ErrorMessages.FailedToUploadFileToServer, ex);
        }

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

            throw new Exception(ErrorMessages.FailedToSaveFileToDatabase, ex);
        }

        return _mapper.Map<ReceiptReadDto>(receipt);
    }

    public async Task<ReceiptReadDto> ProcessReceiptAsync(int receiptId, int userId)
    {
        try
        {
            var existing = await _receiptRepository.FindByIdAsync(receiptId, userId)
                ?? throw new FileNotFoundException(ErrorMessages.ReceiptNotFound(receiptId));

            var fullImagePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
            if (!File.Exists(fullImagePath))
                throw new FileNotFoundException(ErrorMessages.ReceiptImageNotFound(receiptId));

            ReceiptCreateDto parsedDto;

            await using (var stream = File.OpenRead(fullImagePath))
            {
                var parser = _parsers.FirstOrDefault(p => p.CanParse(""))
                    ?? throw new InvalidOperationException(ErrorMessages.ParserNotAvailable);

                existing.Status = Receipt.ReceiptStatus.Processing;
                await _receiptRepository.UpdateAsync(existing);

                parsedDto = await parser.ParseAsync(stream);
            }

            // Check for Costco only receipts
            if (!parsedDto.StoreName.Contains(Strings.Costco, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(fullImagePath))
                        File.Delete(fullImagePath);

                    await _receiptRepository.DeleteAsync(receiptId, userId);

                    throw new ArgumentException(ErrorMessages.UploadCostcoReceipt);
                }

            existing.StoreName = parsedDto.StoreName;
            existing.PurchaseDate = DateTime.SpecifyKind(parsedDto.PurchaseDate, DateTimeKind.Utc);
            existing.TotalAmount = parsedDto.TotalAmount;
            existing.TotalNumberOfItems = parsedDto.TotalNumberOfItems;
            existing.Status = Receipt.ReceiptStatus.PendingReview;

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

            return _mapper.Map<ReceiptReadDto>(existing);
        }
        catch (FileNotFoundException) { throw; }
        catch (ArgumentException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex) { 
            throw new Exception(ErrorMessages.FailedToProcessReceipt, ex);
        }
    }

    public async Task<ReceiptReadDto> GetReceiptPreviewAsync(int receiptId, int userId)
    {
        var existing = await _receiptRepository.FindByIdAsync(receiptId, userId)
            ?? throw new Exception(ErrorMessages.ReceiptNotFound(receiptId));

        if (existing.Status is not (Receipt.ReceiptStatus.PendingReview
                             or Receipt.ReceiptStatus.Processing
                             or Receipt.ReceiptStatus.Processed))
        {
            throw new Exception(ErrorMessages.ReceiptNotReadyForPreview);
        }

        var previewDto = _mapper.Map<ReceiptReadDto>(existing);
        return previewDto;
    }

    public async Task<ReceiptReadDto> ConfirmReceiptAsync(int receiptId, int userId, ReceiptConfirmDto receiptConfirmDto)
    {
        var existing = await _receiptRepository.FindByIdAsync(receiptId, userId)
        ?? throw new Exception(ErrorMessages.ReceiptNotFound(receiptId));

        existing.StoreName = receiptConfirmDto.StoreName;
        existing.PurchaseDate = DateTime.SpecifyKind(receiptConfirmDto.PurchaseDate, DateTimeKind.Utc);
        existing.TotalAmount = receiptConfirmDto.TotalAmount;
        existing.TotalNumberOfItems = receiptConfirmDto.Items.Count;

        existing.Status = Receipt.ReceiptStatus.Processed;

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

        // **TODO** Again needs to be updated, this is not accurate since we dont include depoists / fees
        existing.TotalAmount = existing.Items.Sum(i => i.FinalPrice * i.Quantity);

        await _receiptRepository.UpdateAsync(existing);

        var resultDto = _mapper.Map<ReceiptReadDto>(existing);
        return resultDto;
    }
    public async Task<IReadOnlyList<ReceiptDashboardDto>> GetAllReceiptsForUserAsync(int userId)
    {
        var allReceipts = await _receiptRepository.GetAllByUserAsync(userId);

        if (allReceipts == null || allReceipts.Count == 0)
            return Array.Empty<ReceiptDashboardDto>();

        var mappedReceipts = _mapper.Map<IReadOnlyList<ReceiptDashboardDto>>(allReceipts);

        return mappedReceipts;
    }
    public async Task<ReceiptViewDto> FindByIdAsync(int receiptId, int userId)
    {
        var found = await _receiptRepository.FindByIdAsync(receiptId, userId);

        if (found == null)
            throw new Exception(ErrorMessages.ReceiptNotFound(receiptId));

        var receiptDto = _mapper.Map<ReceiptViewDto>(found);

        return receiptDto;
    }
    public async Task<bool> DeleteAsync(int receiptId, int userId)
    {
        var existing = await _receiptRepository.FindByIdAsync(receiptId, userId);

        if (existing == null)
            throw new FileNotFoundException(ErrorMessages.ReceiptNotFound(receiptId));

        var fullImagePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));

        try
        {
            var success = await _receiptRepository.DeleteAsync(receiptId, userId);
            if (!success)
                throw new Exception(ErrorMessages.FailedToDeleteReceipt(receiptId));

            if (File.Exists(fullImagePath))
                File.Delete(fullImagePath);

            return success;
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorMessages.FailedToDeleteReceipt(receiptId));
        }

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
