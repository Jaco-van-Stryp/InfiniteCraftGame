using System.ComponentModel.DataAnnotations;

namespace InfiniteCraftGame.Infrastructure.Entities;

public class UserWords
{
    public Guid Id { get; init; }

    [MaxLength(255)]
    public required string WordUnlocked { get; init; }

    [MaxLength(10)]
    public string? Emoji { get; init; }

    public Guid? UserId { get; init; }
}
