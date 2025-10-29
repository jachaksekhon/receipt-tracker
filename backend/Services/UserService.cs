using ReceiptTracker.Models;
using ReceiptTracker.Repositories;

namespace ReceiptTracker.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        if (user == null) 
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is required.", nameof(user));

        var existingUser = await _userRepository.FindByEmailAsync(user.Email);

        if(existingUser != null)
            throw new InvalidOperationException($"User with email {user.Email} already exists.");

        return await _userRepository.CreateAsync(user);
    }

    public async Task<IReadOnlyList<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var user = await _userRepository.FindByEmailAsync(email);

        if (user == null)
            throw new Exception($"User with email '{email}' does not exist");

        return user;
    }

    public async Task<User?> FindByIdAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);

        if (user == null)
            throw new Exception($"User with id '{id}' does not exist");

        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);

        if (user == null)
            throw new Exception($"User with id '{id}' does not exist");

        return await _userRepository.DeleteAsync(id);
    }
}
