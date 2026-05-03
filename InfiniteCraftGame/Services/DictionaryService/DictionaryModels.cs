namespace InfiniteCraftGame.Services.DictionaryService;

public readonly record struct DictionaryResult(
    string Word,
    string? Phonetic,
    List<WordMeaning> Meanings
);

public readonly record struct WordMeaning(string PartOfSpeech, List<WordDefinition> Definitions);

public readonly record struct WordDefinition(string Definition);
