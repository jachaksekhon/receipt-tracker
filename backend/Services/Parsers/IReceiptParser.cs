using ReceiptTracker.DTOs.Receipts;

namespace ReceiptTracker.Services.Parsers;

public interface IReceiptParser
{
    bool CanParse(string ocrText);
    Task<ReceiptCreateDto> ParseAsync(Stream imageStream);
}
