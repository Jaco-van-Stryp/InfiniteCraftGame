using System.Net;
using System.Text.Json;

namespace InfiniteCraftGame.Services.DictionaryService;

public class DictionaryService(HttpClient httpClient) : IDictionaryService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<DictionaryResult?> GetDefinitionAsync(
        string word,
        CancellationToken ct = default
    )
    {
        var response = await httpClient.GetAsync($"entries/en/{Uri.EscapeDataString(word)}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var entries = await response.Content.ReadFromJsonAsync<List<DictionaryResult>>(
            JsonOptions,
            ct
        );
        return entries?.FirstOrDefault();
    }
}
