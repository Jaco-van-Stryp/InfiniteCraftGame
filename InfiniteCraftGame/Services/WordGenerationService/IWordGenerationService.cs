namespace InfiniteCraftGame.Services.WordGenerationService;

public interface IWordGenerationService
{
    Task<string> GenerateWord(string wordOne, string wordTwo);
}
