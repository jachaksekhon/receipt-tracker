using ReceiptTracker.DTOs.Receipts;
using ReceiptTracker.Models.Receipts;

namespace ReceiptTracker.Services.Receipts;

public interface IReceiptService
{
    Task<ReceiptReadDto> ProcessReceiptAsync(ReceiptUploadDto uploadDto, int userId);
    Task<IReadOnlyList<Receipt>> GetAllReceiptsForUserAsync(int userId);
    Task<ReceiptReadDto> FindByIdAsync(int id);
    Task<bool> DeleteAsync(int id, int userId);
}
