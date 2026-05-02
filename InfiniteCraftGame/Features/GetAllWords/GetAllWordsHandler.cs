using Bogus;
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
            var faker = new Faker();
            for (var i = 0; i < 10; i++)
            {
                var newWord = new GetAllWordsResponse(
                    Id: faker.Random.Guid(),
                    Word: faker.Random.Word()
                );
                listWords.Add(newWord);
            }
        }

        listWords.AddRange(
            words.Select(word => new GetAllWordsResponse(Id: word.Id, Word: word.WordUnlocked))
        );

        return listWords;
    }
}
