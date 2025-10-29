using ReceiptTracker.Models;

namespace ReceiptTracker.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<IReadOnlyList<User>> GetAllUsersAsync();
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}
