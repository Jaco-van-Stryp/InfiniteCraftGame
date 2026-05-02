using InfiniteCraftGame.Features.CombineWord;

namespace InfiniteCraftGame.Services.WordGenerationService;

public interface IWordGenerationService
{
    Task<CombineWordResponse> GenerateWord(string wordOne, string wordTwo, Guid userId);
}
