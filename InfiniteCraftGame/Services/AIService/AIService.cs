using OpenAI.Chat;

namespace InfiniteCraftGame.Services.AIService;

public class AiService : IAiService
{
    private const string BaseUrl = "https://api.x.ai/v1";
    private readonly string _apiKey;
    private readonly ChatClient _chat;
    private readonly IHttpClientFactory _http;

    public Task<string> GenerateTextAsync(string input)
    {
        throw new NotImplementedException();
    }
}
