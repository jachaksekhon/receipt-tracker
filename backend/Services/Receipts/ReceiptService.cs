using ReceiptTracker.DTOs.Receipts;
using ReceiptTracker.Models.Receipts;
using ReceiptTracker.Repositories.Receipts;
using ReceiptTracker.Services.Parsers;

namespace ReceiptTracker.Services.Receipts;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IEnumerable<IReceiptParser> _parser;

    public ReceiptService(IReceiptRepository receiptRepository, IEnumerable<IReceiptParser> parser)
    {
        _receiptRepository = receiptRepository;
        _parser = parser;
    }
    public Task<ReceiptReadDto> ProcessReceiptAsync(ReceiptUploadDto uploadDto, int userId)
    {
        throw new NotImplementedException();
    }
    public Task<IReadOnlyList<Receipt>> GetAllReceiptsForUserAsync(int userId)
    {
        throw new NotImplementedException();
    }
    public Task<ReceiptReadDto> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    public Task<bool> DeleteAsync(int id, int userId)
    {
        throw new NotImplementedException();
    }

}
