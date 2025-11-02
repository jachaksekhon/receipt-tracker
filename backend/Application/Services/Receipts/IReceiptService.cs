using ReceiptTracker.Application.DTOs.Receipts;
using ReceiptTracker.Domain.Models.Receipts;

namespace ReceiptTracker.Application.Services.Receipts;

public interface IReceiptService
{
    Task<ReceiptReadDto> UploadReceiptAsync(ReceiptUploadDto uploadDto, int userId);
    Task<ReceiptReadDto> ProcessReceiptAsync(int receiptId, int userId);
    Task<ReceiptReadDto> GetReceiptPreviewAsync(int receiptId, int userId);
    Task<ReceiptReadDto> ConfirmReceiptAsync(int receiptId, int userId, ReceiptConfirmDto receiptConfirmDto);
    Task<IReadOnlyList<ReceiptReadDto>> GetAllReceiptsForUserAsync(int userId);
    Task<ReceiptReadDto> FindByIdAsync(int id);
    Task<bool> DeleteAsync(int id, int userId);
}
