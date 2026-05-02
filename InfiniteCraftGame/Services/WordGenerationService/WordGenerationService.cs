using InfiniteCraftGame.Features.CombineWord;
using InfiniteCraftGame.Infrastructure.Data;
using InfiniteCraftGame.Infrastructure.Entities;
using InfiniteCraftGame.Services.AIService;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InfiniteCraftGame.Services.WordGenerationService;

public class WordGenerationService(AppDbContext context, IAiService aiService)
    : IWordGenerationService
{
    public async Task<CombineWordResponse> GenerateWord(string wordOne, string wordTwo)
    {
        return await GetWordCombination(wordOne, wordTwo);
    }

    private async Task<CombineWordResponse> GetWordCombination(string wordOne, string wordTwo)
    {
        var w1 = wordOne.ToLower();
        var w2 = wordTwo.ToLower();
        var word = await context.WordCombinations.FirstOrDefaultAsync(x =>
            (x.WordOne.ToLower() == w1 && x.WordTwo.ToLower() == w2)
            || (x.WordOne.ToLower() == w2 && x.WordTwo.ToLower() == w1)
        );

        if (word == null)
        {
            return new CombineWordResponse(
                WordCombination: await CombineWordsAsync(wordOne, wordTwo),
                FirstDiscovery: true
            );
        }

        return new CombineWordResponse(WordCombination: word.WordCombined, FirstDiscovery: false);
    }

    private async Task<string> CombineWordsAsync(string wordOne, string wordTwo)
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
        if (string.IsNullOrWhiteSpace(result) || result.Any(char.IsWhiteSpace))
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
        await context.SaveChangesAsync();
        return result;
    }

    private string InfiniteCraftSystemPrompt() =>
        """
            You are the crafting engine for a word-fusion game like Infinite Craft. Given "A + B", output the single most fitting result.

            Rules:
            - Output EXACTLY ONE token: a single word or established PascalCase name. No spaces, punctuation, quotes, or explanation.
            - The result MUST be a real, recognizable thing — a dictionary word, place, named character, brand, phenomenon, food, myth, or pop-culture reference. If a literate adult wouldn't recognize it without you explaining, it's wrong.
            - NEVER glue the inputs together into a new compound. Frog + Pineapple ≠ PineappleFrog; pick Jungle, TreeFrog, or SpongeBob. Cat + Sky ≠ SkyCat; pick Kite or Pegasus. Fire + Coffee ≠ FireCoffee; pick Espresso.
            - Compounds are allowed only when the compound itself is the real name (IronMan, BlackHole, MountEverest, PoisonDartFrog). When in doubt, pick a single word.
            - Prefer the iconic "of course" answer over the clever one. Stay grounded — don't leap tiers (Mud + Fire = Brick, not Castle).
            - Commutative: A + B = B + A.
            - No tautologies (Water + Wet ≠ Water; pick Mist or Puddle). No vague categories (Thing, Stuff, Animal).
            - Draw from physics, biology, mythology, history, film, games, music, cuisine, idioms, religion, and folklore.
            - Treat misspellings charitably — interpret each input as the closest real word or name (e.g. "patric" → Patrick, "retart" → Restart, "spongbob" → SpongeBob) and craft from the corrected meaning.
            - ALWAYS produce a real, creative result. Never bail. Every pair has a fitting fusion — if the answer isn't obvious, ask: what category contains both? what idiom or phrase links them? what character, brand, place, or process uses both? what does A do TO B, or B do TO A? Pick the strongest connection and commit. Do NOT respond with "Nothing", "None", "Unknown", or any refusal, and never echo the inputs glued together.

            Examples:
            Fire + Water → Steam
            Mud + Fire → Brick
            Human + Fish → Mermaid
            Wood + Boy → Pinocchio
            Bat + Man → Batman
            Sun + Moon → Eclipse
            Frog + Pineapple → SpongeBob

            Respond with only the result.
            """;
}
