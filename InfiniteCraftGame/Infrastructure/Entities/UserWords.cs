using System.ComponentModel.DataAnnotations;

namespace InfiniteCraftGame.Infrastructure.Entities;

public class UserWords
{
    public Guid Id { get; init; }

    [MaxLength(255)]
    public required string WordUnlocked { get; init; }

    public User? User { get; init; } = null!;
    public Guid? UserId { get; init; }
}
