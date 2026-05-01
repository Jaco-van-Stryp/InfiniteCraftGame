using System.ComponentModel.DataAnnotations;

namespace InfiniteCraftGame.Infrastructure.Entities;

public class User
{
    public IEnumerable<UserWords> UserWords = new List<UserWords>();
    public IEnumerable<WordCombinations> WordCombinations = new List<WordCombinations>();
    public Guid Id { get; init; }

    [MaxLength(255)]
    public required string Username { get; init; }
}
