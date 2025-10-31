using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using ReceiptTracker.Application.DTOs.Auth;
using ReceiptTracker.Data;
using ReceiptTracker.Domain.Models;
using ReceiptTracker.Infrastructure.Repositories.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReceiptTracker.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<User> RegisterAsync(UserRegisterDto request)
    {
        var existingUser = await _userRepository.FindByEmailAsync(request.Email);
        if (existingUser != null) 
            throw new Exception("User with this email already exists");

        CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

        var user = new User
        {
            FirstName    = request.FirstName,
            LastName     = request.LastName,
            Email        = request.Email,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        await _userRepository.CreateAsync(user);

        return user;
    }

    public async Task<string> LoginAsync(UserLoginDto request)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("User not found.");

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Invalid password.");

        return CreateToken(user);
    }

    public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepository.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("User not found.");

        // TODO: Improve RNG, make a secure random string
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToBase64String(tokenBytes);

        user.ResetPasswordToken = token;
        user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

        await _userRepository.UpdateAsync(user);

        ////////// **************** TODO: ADD IN EMAIL MESSAGE WITH TOKEN FOR USERS SO THEY CAN RESET PASSWORD **************** //////////
        Console.WriteLine($"Password reset token for {user.Email}: {token}");

        return token;
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = (await _userRepository.GetAllAsync())
            .FirstOrDefault(u => u.ResetPasswordToken == dto.Token && u.ResetTokenExpires > DateTime.UtcNow);

        if (user == null)
            throw new Exception("Invalid or expired reset token.");

        CreatePasswordHash(dto.NewPassword, out byte[] hash, out byte[] salt);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
        user.ResetPasswordToken = null;
        user.ResetTokenExpires = null;

        await _userRepository.UpdateAsync(user);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found.");

        if (!VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Current password is incorrect.");

        CreatePasswordHash(dto.NewPassword, out byte[] newHash, out byte[] newSalt);
        user.PasswordHash = newHash;
        user.PasswordSalt = newSalt;

        await _userRepository.UpdateAsync(user);
    }

    // *** HELPER METHODS ***

    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
