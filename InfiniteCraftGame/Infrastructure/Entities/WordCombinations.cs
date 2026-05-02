using System.ComponentModel.DataAnnotations;

namespace InfiniteCraftGame.Infrastructure.Entities;

public class WordCombinations
{
    public Guid Id { get; init; }

    [MaxLength(255)]
    public required string WordOne { get; init; }

    [MaxLength(255)]
    public required string WordTwo { get; init; }

    [MaxLength(255)]
    public required string WordCombined { get; init; }

    [MaxLength(10)]
    public string? Emoji { get; init; }

    public Guid? DiscoveredById { get; init; }
}
