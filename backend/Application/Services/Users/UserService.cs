using AutoMapper;
using ReceiptTracker.Application.DTOs.Users;
using ReceiptTracker.Domain.Models;
using ReceiptTracker.Infrastructure.Repositories.Users;

namespace ReceiptTracker.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<UserReadDto?> FindByEmailAsync(string email)
    {
        var user = await _userRepository.FindByEmailAsync(email);

        if (user == null)
            throw new Exception($"User with email '{email}' does not exist");

        var userReadDto = _mapper.Map<UserReadDto>(user);

        return userReadDto;
    }

    public async Task<UserReadDto?> FindByIdAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);

        if (user == null)
            throw new Exception($"User with id '{id}' does not exist");

        var userReadDto = _mapper.Map<UserReadDto>(user);

        return userReadDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);

        if (user == null)
            throw new Exception($"User with id '{id}' does not exist");

        return await _userRepository.DeleteAsync(id);
    }
}
