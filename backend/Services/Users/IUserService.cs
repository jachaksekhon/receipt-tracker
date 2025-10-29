using ReceiptTracker.Models;

namespace ReceiptTracker.Services.Users;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllUsersAsync();
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}
