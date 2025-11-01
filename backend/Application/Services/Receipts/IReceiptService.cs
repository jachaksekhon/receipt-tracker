using ReceiptTracker.Application.DTOs.Receipts;
using ReceiptTracker.Domain.Models.Receipts;

namespace ReceiptTracker.Application.Services.Receipts;

public interface IReceiptService
{
    Task<ReceiptReadDto> UploadReceiptAsync(ReceiptUploadDto uploadDto, int userId);
    //Task<ReceiptReadDto> ProcessReceiptAsync(int receiptId, int userId);
    Task<IReadOnlyList<ReceiptReadDto>> GetAllReceiptsForUserAsync(int userId);
    Task<ReceiptReadDto> FindByIdAsync(int id);
    Task<bool> DeleteAsync(int id, int userId);
}
