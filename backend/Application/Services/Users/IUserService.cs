using ReceiptTracker.Application.DTOs.Users;
using ReceiptTracker.Domain.Models;

namespace ReceiptTracker.Application.Services.Users;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllUsersAsync();
    Task<UserReadDto?> FindByEmailAsync(string email);
    Task<UserReadDto?> FindByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}
