using MediatR;

namespace InfiniteCraftGame.Features.CombineWord;

public static class CombineWordEndpoint
{
    public static void MapCombineWordEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "CombineWord",
                async (CombineWordCommand command, ISender sender) =>
                {
                    var response = await sender.Send(command);
                    return TypedResults.Ok(response);
                }
            )
            .WithName("CombineWord");
    }
}
