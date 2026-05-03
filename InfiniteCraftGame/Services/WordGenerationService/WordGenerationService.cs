using InfiniteCraftGame.Features.CombineWord;
using InfiniteCraftGame.Infrastructure.Data;
using InfiniteCraftGame.Infrastructure.Entities;
using InfiniteCraftGame.Services.AIService;
using InfiniteCraftGame.Services.DictionaryService;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InfiniteCraftGame.Services.WordGenerationService;

public class WordGenerationService(
    AppDbContext context,
    IAiService aiService,
    IDictionaryService dictionaryService
) : IWordGenerationService
{
    public async Task<CombineWordResponse> GenerateWord(string wordOne, string wordTwo, Guid userId)
    {
        return await GetWordCombination(wordOne, wordTwo, userId);
    }

    private async Task<CombineWordResponse> GetWordCombination(
        string wordOne,
        string wordTwo,
        Guid userId
    )
    {
        var w1 = wordOne.ToLower();
        var w2 = wordTwo.ToLower();
        var word = await context.WordCombinations.FirstOrDefaultAsync(x =>
            (x.WordOne.ToLower() == w1 && x.WordTwo.ToLower() == w2)
            || (x.WordOne.ToLower() == w2 && x.WordTwo.ToLower() == w1)
        );

        if (word == null)
        {
            var (combined, emoji, definition) = await CombineWordsAsync(wordOne, wordTwo, userId);
            return new CombineWordResponse(
                WordCombination: combined,
                Emoji: emoji,
                FirstDiscovery: true,
                Definition: definition
            );
        }

        return new CombineWordResponse(
            WordCombination: word.WordCombined,
            Emoji: word.Emoji ?? "✨",
            FirstDiscovery: false,
            Definition: word.Definition
        );
    }

    private async Task<(string Word, string Emoji, string? Definition)> CombineWordsAsync(
        string wordOne,
        string wordTwo,
        Guid userId
    )
    {
        var result = await aiService.GenerateTextAsync(
            InfiniteCraftSystemPrompt(),
            $"{wordOne} + {wordTwo}"
        );
        Log.Information(
            "Generated Word - Word One: '{WordOne}' Word Two: '{WordTwo}' Combination: '{Result}'",
            wordOne,
            wordTwo,
            result
        );

        var parts = result.Split(':');
        var word = parts[0].Trim();
        var emoji = parts.Length > 1 ? parts[1].Trim() : "✨";

        if (string.IsNullOrWhiteSpace(word) || word.Any(char.IsWhiteSpace))
            return ("Nothing", "❓", null);

        var dictionaryResult = await dictionaryService.GetDefinitionAsync(word);
        var definition = dictionaryResult
            ?.Meanings.FirstOrDefault()
            .Definitions?.FirstOrDefault()
            .Definition;

        var combinedWord = new WordCombinations
        {
            WordOne = wordOne,
            WordTwo = wordTwo,
            WordCombined = word,
            Emoji = emoji,
            DiscoveredById = userId,
            Definition = definition,
        };

        var userWord = new UserWords
        {
            WordUnlocked = word,
            Emoji = emoji,
            UserId = userId,
            Definition = definition,
        };
        await context.UserWords.AddAsync(userWord);
        await context.WordCombinations.AddAsync(combinedWord);
        await context.SaveChangesAsync();
        return (word, emoji, definition);
    }

    private string InfiniteCraftSystemPrompt() =>
        """
            Word-fusion game. Given "A + B", output the single best result as word:emoji.
            - Format: one word (or real PascalCase name) followed by a colon and one relevant emoji. Nothing else.
            - Must be a real, recognizable thing. NEVER glue inputs (Frog+Pineapple→SpongeBob:🧽, not PineappleFrog). Compounds only if the name is real (IronMan:🦸, BlackHole:🌑).
            - Prefer the obvious answer. No tautologies. Never refuse.
            """;
}
