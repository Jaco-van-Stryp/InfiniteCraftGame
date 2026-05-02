using InfiniteCraftGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InfiniteCraftGame.Features.GetAllWords;

public class GetAllWordsHandler(AppDbContext context)
    : IRequestHandler<GetAllWordsQuery, List<GetAllWordsResponse>>
{
    public async Task<List<GetAllWordsResponse>> Handle(
        GetAllWordsQuery request,
        CancellationToken cancellationToken
    )
    {
        var words = await context
            .UserWords.Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);
        var listWords = new List<GetAllWordsResponse>();
        if (words.Count == 0)
        {
            listWords.AddRange([
                new GetAllWordsResponse(Id: Guid.NewGuid(), Word: "Seed", Emoji: "🌱"),
                new GetAllWordsResponse(Id: Guid.NewGuid(), Word: "Spark", Emoji: "⚡"),
                new GetAllWordsResponse(Id: Guid.NewGuid(), Word: "Clay", Emoji: "🏺"),
                new GetAllWordsResponse(Id: Guid.NewGuid(), Word: "Song", Emoji: "🎵"),
                new GetAllWordsResponse(Id: Guid.NewGuid(), Word: "Coin", Emoji: "🪙"),
                new GetAllWordsResponse(Id: Guid.NewGuid(), Word: "Dream", Emoji: "💭"),
            ]);
        }

        listWords.AddRange(
            words.Select(word => new GetAllWordsResponse(
                Id: word.Id,
                Word: word.WordUnlocked,
                Emoji: word.Emoji ?? "✨"
            ))
        );

        return listWords;
    }
}
