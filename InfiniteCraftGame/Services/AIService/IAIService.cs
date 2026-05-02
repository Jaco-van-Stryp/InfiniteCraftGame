namespace InfiniteCraftGame.Services.AIService;

public interface IAiService
{
    Task<string> GenerateTextAsync(
        string systemPrompt,
        string userMessage,
        CancellationToken ct = default
    );
}
