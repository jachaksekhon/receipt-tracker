using ReceiptTracker.Models;
using ReceiptTracker.Models.Receipts;

namespace ReceiptTracker.Repositories.Receipts;

public interface IReceiptRepository
{
    Task<User> CreateAsync(Receipt receipt);
    Task<User?> UpdateAsync(Receipt receipt);
    Task<bool> DeleteAsync(int id);
    Task<User?> FindByIdAsync(int id);
    Task<IReadOnlyList<User>> GetAllAsync();
}
