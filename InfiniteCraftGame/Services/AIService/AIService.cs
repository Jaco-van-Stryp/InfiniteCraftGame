using Anthropic;
using Anthropic.Models.Messages;
using Microsoft.Extensions.Options;

namespace InfiniteCraftGame.Services.AIService;

public class AiService(IOptions<AiServiceOptions> options) : IAiService
{
    private const string ModelId = "claude-haiku-4-5";
    private readonly AnthropicClient _client = new() { ApiKey = options.Value.ApiKey };

    public async Task<string> GenerateTextAsync(
        string systemPrompt,
        string userMessage,
        CancellationToken ct = default
    )
    {
        try
        {
            var response = await _client.Messages.Create(
                new MessageCreateParams
                {
                    Model = ModelId,
                    MaxTokens = 64,
                    System = new List<TextBlockParam>
                    {
                        new() { Text = systemPrompt, CacheControl = new CacheControlEphemeral() },
                    },
                    Messages = [new MessageParam { Role = Role.User, Content = userMessage }],
                },
                ct
            );

            return response.Content.Select(b => b.Value).OfType<TextBlock>().FirstOrDefault()?.Text
                ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Anthropic chat failed using model '{ModelId}': {ex.Message}",
                ex
            );
        }
    }
}
