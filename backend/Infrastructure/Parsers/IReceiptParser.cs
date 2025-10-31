using ReceiptTracker.Application.DTOs.Receipts;

namespace ReceiptTracker.Application.Parsers;

public interface IReceiptParser
{
    bool CanParse(string ocrText);
    Task<ReceiptCreateDto> ParseAsync(Stream imageStream);
}
