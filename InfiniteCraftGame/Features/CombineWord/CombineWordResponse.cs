namespace InfiniteCraftGame.Features.CombineWord;

public readonly record struct CombineWordResponse(
    string WordCombination,
    string Emoji,
    bool FirstDiscovery
);
