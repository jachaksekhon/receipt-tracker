using ReceiptTracker.Models;

namespace ReceiptTracker.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<IReadOnlyList<User>> GetAllUsersAsync();
    Task<User> FindByEmailAsync(string email);
}
