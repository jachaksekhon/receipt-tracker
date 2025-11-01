using ReceiptTracker.Infrastructure.Parsers;

public static class TestParser
{
    public static async Task RunAsync()
    {
        Console.WriteLine("🧠 Testing OpenAI Receipt Parser...");

        var parser = new OpenAiReceiptParser();

        // 🖼 Path to your saved image
        var imagePath = @"H:\Downloads\Projects\receipt-tracker\backend\wwwroot\uploads\3f47a760-2398-4643-b2ba-7545ee0309fd_CostcoReceipt.jpeg";
        await using var stream = File.OpenRead(imagePath);

        try
        {
            var dto = await parser.ParseAsync(stream);

            //Console.WriteLine("\n✅ Model returned structured data:\n");
            //Console.WriteLine($"Store: {dto.StoreName}");
            //Console.WriteLine($"Purchase Date: {dto.PurchaseDate}");
            //Console.WriteLine($"Total: {dto.TotalAmount}");
            //Console.WriteLine($"Item Count: {dto.TotalNumberOfItems}");

            //Console.WriteLine("\nItems:");
            //if (dto.Items != null)
            //{
            //    foreach (var item in dto.Items)
            //    {
            //        Console.WriteLine($"- {item.ProductSku} | {item.ItemName} | ${item.Price}");
            //    }
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }
}
