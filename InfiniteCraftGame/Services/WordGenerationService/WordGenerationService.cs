using InfiniteCraftGame.Infrastructure.Data;
using InfiniteCraftGame.Infrastructure.Entities;
using InfiniteCraftGame.Services.AIService;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;

namespace InfiniteCraftGame.Services.WordGenerationService;

public class WordGenerationService(AppDbContext context, IAiService aiService)
    : IWordGenerationService
{
    public async Task<string> GenerateWord(string wordOne, string wordTwo)
    {
        return await GetWordCombination(wordOne, wordTwo);
    }

    private async Task<string> GetWordCombination(string wordOne, string wordTwo)
    {
        var word = await context.WordCombinations.FirstOrDefaultAsync(x =>
            (
                x.WordOne.Equals(wordOne, StringComparison.CurrentCultureIgnoreCase)
                && x.WordTwo.Equals(wordTwo, StringComparison.CurrentCultureIgnoreCase)
            )
            || (
                x.WordOne.Equals(wordTwo, StringComparison.CurrentCultureIgnoreCase)
                && x.WordTwo.Equals(wordOne, StringComparison.CurrentCultureIgnoreCase)
            )
        );
        if (word == null)
        {
            return await CombineWordsAsync(wordOne, wordTwo);
        }
        else
        {
            return word.WordCombined;
        }
    }

    private async Task<string> CombineWordsAsync(string wordOne, string wordTwo)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(InfiniteCraftSystemPrompt()),
            new UserChatMessage($"{wordOne} + {wordTwo}"),
        };

        var result = await aiService.GenerateTextAsync(messages);
        if (result.Length != 1)
            return "Nothing";
        var combinedWord = new WordCombinations
        {
            WordOne = wordOne,
            WordTwo = wordTwo,
            WordCombined = result,
        }; // TODO - Add user tracking here

        var userWord = new UserWords { WordUnlocked = result }; //TODO - Add user tracking here
        await context.UserWords.AddAsync(userWord);
        await context.WordCombinations.AddAsync(combinedWord);
        return result;
    }

    private string InfiniteCraftSystemPrompt() =>
        """
            You are an Infinite Craft AI. Your job is to combine two elements into a single new element.

            Rules:
            - Always respond with **exactly one word** (or a short compound word/phrase if it makes sense, like "BlackHole" or "TimeMachine").
            - Be creative, clever, and fun.
            - Combine the two inputs logically, metaphorically, or humorously.
            - You can invent completely new concepts if they feel like a natural combination.
            - Use title case (e.g. "Rainbow", "DragonFruit", "LoveLetter").
            - Do not explain your answer. Do not add any extra text.

            Examples:
            - Water + Fire → Steam
            - Earth + Plant → Tree
            - Love + Time → Eternity
            - Cat + Internet → CatVideo
            - Bird + Computer → Twitter
            """;
}
