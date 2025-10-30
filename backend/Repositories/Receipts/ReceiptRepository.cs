using ReceiptTracker.Data;

namespace ReceiptTracker.Repositories.Receipts;

public class ReceiptRepository : IReceiptRepository
{
    private readonly AppDbContext _context;

    public ReceiptRepository(AppDbContext context)
    {
        _context = context;
    }


}
