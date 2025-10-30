using ReceiptTracker.Data;

namespace ReceiptTracker.Repositories.ReceiptItems;

public class ReceiptItemRepository : IReceiptItemRepository
{
    private readonly AppDbContext _context;

    public ReceiptItemRepository(AppDbContext context)
    {
        _context = context;
    }
}
