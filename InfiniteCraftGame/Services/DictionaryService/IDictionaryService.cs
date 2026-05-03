namespace InfiniteCraftGame.Services.DictionaryService;

public interface IDictionaryService
{
    Task<DictionaryResult?> GetDefinitionAsync(string word, CancellationToken ct = default);
}
