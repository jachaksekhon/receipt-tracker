using AutoMapper;
using ReceiptTracker.Application.Constants;
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

    public async Task<IReadOnlyList<UserReadDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<UserReadDto>>(users);
    }

    public async Task<UserReadDto?> FindByEmailAsync(string email)
    {
        var user = await _userRepository.FindByEmailAsync(email);

        if (user == null)
            throw new Exception(ErrorMessages.UserNotFoundByEmail(email));

        var userReadDto = _mapper.Map<UserReadDto>(user);

        return userReadDto;
    }

    public async Task<UserReadDto?> FindByIdAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);

        if (user == null)
            throw new Exception(ErrorMessages.UserNotFoundbyId(id));

        var userReadDto = _mapper.Map<UserReadDto>(user);

        return userReadDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);

        if (user == null)
            throw new Exception(ErrorMessages.UserNotFoundbyId(id));

        return await _userRepository.DeleteAsync(id);
    }
}
