using Microsoft.AspNetCore.Mvc.Formatters;
using ReceiptTracker.Application.DTOs.ReceiptItems;
using ReceiptTracker.Application.DTOs.Receipts;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace ReceiptTracker.Infrastructure.Parsers;

public static class ParsedReceiptMapper
{
    public static ReceiptCreateDto MapToReceiptCreateDto(ParsedReceiptDto parsed)
    {
        if (parsed == null)
            throw new ArgumentNullException(nameof(parsed));

        var newReceiptDto = new ReceiptCreateDto
        {
            StoreName = parsed.StoreName ?? "",
            PurchaseDate = parsed.Date ?? DateTime.UtcNow,
            Items = new List<ReceiptItemCreateDto>()
        };

        var skuLookup = new Dictionary<string, ReceiptItemCreateDto>(StringComparer.OrdinalIgnoreCase);

        ReceiptItemCreateDto? lastProduct = null;

        foreach (var item in parsed.Items)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.RawText))
                continue;

            // Handling discounts

            if (item.IsNegative)
            {
                string? discountTargetSku = ExtractSkuFromTPD(item.RawText);

                if (!string.IsNullOrEmpty(discountTargetSku) && skuLookup.TryGetValue(discountTargetSku, out var target))
                {
                    target.DiscountAmount += Math.Abs(item.Price);
                }
                else if (lastProduct != null)
                {
                    lastProduct.DiscountAmount += Math.Abs(item.Price);
                }

                continue;
            }

            var newReceiptItem = new ReceiptItemCreateDto
            {
                ItemName = item.ExpandedName ?? item.RawText,
                ProductSku = item.Sku,
                Quantity = 1,
                OriginalPrice = item.Price,
                DiscountAmount = 0,
                FinalPrice = Math.Abs(item.Price)
            };

            newReceiptDto.Items.Add(newReceiptItem);
            lastProduct = newReceiptItem;

            if (!string.IsNullOrEmpty(item.Sku))
                skuLookup[item.Sku] = newReceiptItem;
        }   

        // update total price after applying discount

        foreach (var item in newReceiptDto.Items)
        {
            item.FinalPrice = item.FinalPrice - item.DiscountAmount;
        }

        newReceiptDto.TotalAmount = newReceiptDto.Items.Sum(i => i.FinalPrice);
        newReceiptDto.TotalNumberOfItems = newReceiptDto.Items.Count;


        return newReceiptDto;
    }

    private static string? ExtractSkuFromTPD(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var match = Regex.Match(text, @"TPD/(\d+)");
        return match.Success ? match.Groups[1].Value : null;
    }
}
