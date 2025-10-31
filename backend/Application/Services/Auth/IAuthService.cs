using ReceiptTracker.Application.DTOs.Auth;
using ReceiptTracker.Application.DTOs.Users;
using ReceiptTracker.Domain.Models;

namespace ReceiptTracker.Application.Services.Auth;

public interface IAuthService
{
    Task<UserReadDto> RegisterAsync(UserRegisterDto request);
    Task<string> LoginAsync(UserLoginDto request);
    Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task ResetPasswordAsync(ResetPasswordDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
