using OpenAI.Chat;

namespace InfiniteCraftGame.Services.AIService;

public interface IAiService
{
    Task<string> GenerateTextAsync(
        IEnumerable<ChatMessage> messages,
        CancellationToken ct = default
    );
}
