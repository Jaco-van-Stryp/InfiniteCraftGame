using InfiniteCraftGame.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfiniteCraftGame.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserWords> UserWords { get; set; } = null!;
    public DbSet<WordCombinations> WordCombinations { get; set; } = null!;
}
