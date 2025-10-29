using ReceiptTracker.DTOs.Auth;
using ReceiptTracker.Models;

namespace ReceiptTracker.Services.Auth;

public interface IAuthService
{
    Task<User> RegisterAsync(UserRegisterDto request);
    Task<string> LoginAsync(UserLoginDto request);
    Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task ResetPasswordAsync(ResetPasswordDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
