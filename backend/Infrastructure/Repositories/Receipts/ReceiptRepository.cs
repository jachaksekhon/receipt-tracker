using Microsoft.EntityFrameworkCore;
using ReceiptTracker.Domain.Models.Receipts;
using ReceiptTracker.Infrastructure.Data;

namespace ReceiptTracker.Infrastructure.Repositories.Receipts;

public class ReceiptRepository : IReceiptRepository
{
    private readonly AppDbContext _context;

    public ReceiptRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Receipt> CreateAsync(Receipt receipt)
    {
        _context.Receipts.Add(receipt);
        await _context.SaveChangesAsync();
        return receipt;
    }

    public async Task<Receipt?> FindByIdAsync(int id, int userId)
    {
        return await _context.Receipts
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id  == id && r.UserId == userId);
    }

    public async Task<IReadOnlyList<Receipt>> GetAllByUserAsync(int userId)
    {
        return await _context.Receipts
            .Include(r => r.Items)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.PurchaseDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id, int userid)
    {
        var found = await _context.Receipts
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userid);

        if (found == null)
            return false;

        _context.Receipts.Remove(found);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Receipt?> UpdateAsync(Receipt receipt)
    {
        var existing = await _context.Receipts.FindAsync(receipt.Id);
        if (existing == null)
            return null;

        existing.StoreName = receipt.StoreName;
        existing.PurchaseDate = receipt.PurchaseDate;
        existing.TotalAmount = receipt.TotalAmount;
        existing.Notes = receipt.Notes;

        existing.Items = receipt.Items;

        await _context.SaveChangesAsync();
        return existing;
    }
}
