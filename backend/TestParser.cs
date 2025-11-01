//using ReceiptTracker.Infrastructure.Parsers;
//using System.Text.Json;

//public static class TestParser
//{
//    public static async Task RunAsync()
//    {
//        Console.WriteLine("🧠 Testing OpenAI Receipt Parser...");

//        var parser = new OpenAiReceiptParser();

//        // 🖼 Replace with your test receipt image
//        var imagePath = @"H:\Downloads\Projects\receipt-tracker\backend\wwwroot\uploads\3f47a760-2398-4643-b2ba-7545ee0309fd_CostcoReceipt.jpeg";

//        await using var stream = File.OpenRead(imagePath);

//        try
//        {
//            // 🧾 Run the full parser (includes GPT + mapping)
//            var dto = await parser.ParseAsync(stream);

//            Console.WriteLine("\n✅ Receipt parsed and mapped successfully!");
//            Console.WriteLine($"Store: {dto.StoreName}");
//            Console.WriteLine($"Date: {dto.PurchaseDate:yyyy-MM-dd}");
//            Console.WriteLine($"Items: {dto.TotalNumberOfItems}");
//            Console.WriteLine($"Total: ${dto.TotalAmount:F2}");

//            Console.WriteLine("\n📦 Items:");
//            foreach (var item in dto.Items)
//            {
//                var discountText = item.DiscountAmount > 0
//                    ? $"(Sale -${item.DiscountAmount:F2})"
//                    : "";

//                Console.WriteLine($"- {item.ItemName} | SKU: {item.ProductSku ?? "N/A"} | ${item.OriginalPrice:F2} {discountText} → Paid: ${item.FinalPrice:F2}");
//            }

//            //// 🧩 Optional: Write result to file for debugging
//            //var outputPath = Path.Combine(AppContext.BaseDirectory, "receipt_test_output.json");
//            //await File.WriteAllTextAsync(outputPath, JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true }));

//            //Console.WriteLine($"\n💾 Saved parsed result to: {outputPath}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"\n❌ Error: {ex.Message}");
//        }
//    }
//}
