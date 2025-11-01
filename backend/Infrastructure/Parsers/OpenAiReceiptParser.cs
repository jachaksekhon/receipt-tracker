using AutoMapper;
using OpenAI;
using OpenAI.Chat;
using ReceiptTracker.Application.DTOs.Receipts;
using System.Data;
using System.Text.Json;

namespace ReceiptTracker.Infrastructure.Parsers;

public class OpenAiReceiptParser : IReceiptParser
{
    private readonly ChatClient _chatClient;
    private readonly string _model;
    private readonly string _prompt;

    public OpenAiReceiptParser()
    {
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? throw new InvalidOperationException("Missing OpenAI API key. Check your .env file.");

        _model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

        var promptPath = Path.Combine(AppContext.BaseDirectory, "config", "prompts", "receipt_parser.txt");
        if (!File.Exists(promptPath))
            throw new FileNotFoundException($"Missing prompt file at: {promptPath}");

        _prompt = File.ReadAllText(promptPath);

        _chatClient = new ChatClient(model: _model, apiKey: apiKey);
    }

    public bool CanParse(string ocrText) => true;

    public async Task<ReceiptCreateDto> ParseAsync(Stream imageStream)
    {
        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms);
        var imageBytes = ms.ToArray();


        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful receipt parsing assistant."),
            new UserChatMessage(_prompt),
            new UserChatMessage(new List<ChatMessageContentPart>
            {
                ChatMessageContentPart.CreateImagePart(new BinaryData(imageBytes), "image/jpeg")
            })
        };

        var options = new ChatCompletionOptions
        {
            ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat(),
            MaxOutputTokenCount = 1500
        };

        ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);
        var json = completion.Content.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(json))
            throw new Exception("No valid JSON returned by model.");

        try
        {
            var parsedReceipt = JsonSerializer.Deserialize<ParsedReceiptDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new Exception("Failed to deserialize receipt JSON.");

            var mapped = ParsedReceiptMapper.MapToReceiptCreateDto(parsedReceipt);

            return mapped;
        }
        catch (JsonException ex)
        {
            throw new Exception($"Failed to parse model output:\n{json}", ex);
        }
    }
}
