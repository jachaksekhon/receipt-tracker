using ReceiptTracker.Models;
using ReceiptTracker.Models.Receipts;

namespace ReceiptTracker.Repositories.Receipts;

public interface IReceiptRepository
{
    Task<Receipt> CreateAsync(Receipt receipt);
    Task<Receipt?> FindByIdAsync(int id, int userId);
    Task<IReadOnlyList<Receipt>> GetAllByUserAsync(int userId);
    Task<bool> DeleteAsync(int id, int userId);
    Task<Receipt?> UpdateAsync(Receipt receipt);
}
