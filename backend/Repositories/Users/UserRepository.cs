using ReceiptTracker.Data;
using ReceiptTracker.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace ReceiptTracker.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var currentUserInDb = await _context.Users.FindAsync(user.Id);

        if (currentUserInDb == null)
        {
            return null;
        }

        currentUserInDb.FirstName    = user.FirstName;
        currentUserInDb.LastName     = user.LastName;
        currentUserInDb.Email        = user.Email;

        if (user.PasswordHash != null && user.PasswordSalt != null)
        {
            currentUserInDb.PasswordHash = user.PasswordHash;
            currentUserInDb.PasswordSalt = user.PasswordSalt;
        }

        await _context.SaveChangesAsync();
        return currentUserInDb;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> FindByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }
}
