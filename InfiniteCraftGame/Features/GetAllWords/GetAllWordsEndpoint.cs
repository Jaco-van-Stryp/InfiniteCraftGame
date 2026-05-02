using MediatR;

namespace InfiniteCraftGame.Features.GetAllWords;

public static class GetAllWordsEndpoint
{
    public static void MapGetAllWordsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "GetAllWords",
                async (ISender sender, Guid? userId) =>
                {
                    var results = await sender.Send(new GetAllWordsQuery(userId));
                    return TypedResults.Ok(results);
                }
            )
            .WithName("GetAllWords");
    }
}
