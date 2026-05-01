using System.ClientModel;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace InfiniteCraftGame.Services.AIService;

public class AiService : IAiService
{
    private const string BaseUrl = "https://api.x.ai/v1";
    private readonly ChatClient _chat;

    public AiService(IOptions<AiServiceOptions> options)
    {
        var apiKey = options.Value.ApiKey;

        var client = new OpenAIClient(
            new ApiKeyCredential(apiKey),
            new OpenAIClientOptions { Endpoint = new Uri(BaseUrl) }
        );

        _chat = client.GetChatClient("grok-4-1-fast-non-reasoning");
    }

    public async Task<string> GenerateTextAsync(
        IEnumerable<ChatMessage> messages,
        CancellationToken ct = default
    )
    {
        try
        {
            var options = new ChatCompletionOptions
            {
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat(),
            };

            var result = await _chat.CompleteChatAsync(messages, options, cancellationToken: ct);
            return result.Value.Content[0].Text;
        }
        catch (ClientResultException ex)
        {
            var body = ex.GetRawResponse()?.Content.ToString() ?? "(no body)";
            throw new InvalidOperationException(
                $"xAI chat failed [{ex.Status}] using model grok-4-1-fast-non-reasoning': {body}",
                ex
            );
        }
    }
}
