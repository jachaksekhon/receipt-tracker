using ReceiptTracker.Application.DTOs.Receipts;

namespace ReceiptTracker.Infrastructure.Parsers;

public interface IReceiptParser
{
    bool CanParse(string ocrText);
    Task<ReceiptCreateDto> ParseAsync(Stream imageStream);
}
