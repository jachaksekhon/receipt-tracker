using ReceiptTracker.Domain.Models;

namespace ReceiptTracker.Infrastructure.Repositories.Users;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(int id);
    Task<IReadOnlyList<User>> GetAllAsync();
}
