using ReceiptTracker.Repositories.Receipts;

namespace ReceiptTracker.Services.Receipts;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _receiptRepository;

    public ReceiptService(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }
}
