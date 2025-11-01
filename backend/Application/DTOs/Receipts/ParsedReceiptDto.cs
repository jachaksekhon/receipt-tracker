using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace ReceiptTracker.Application.DTOs.Receipts;

public class ParsedReceiptDto
{
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("storeName")]
    public string? StoreName { get; set; }

    [JsonPropertyName("items")]
    public List<ParsedLineDto> Items { get; set; } = new();
}
