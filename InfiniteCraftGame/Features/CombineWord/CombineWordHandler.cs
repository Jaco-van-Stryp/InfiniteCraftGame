using InfiniteCraftGame.Services.WordGenerationService;
using MediatR;

namespace InfiniteCraftGame.Features.CombineWord;

public class CombineWordHandler(IWordGenerationService wordGenerationService)
    : IRequestHandler<CombineWordCommand, CombineWordResponse>
{
    public async Task<CombineWordResponse> Handle(
        CombineWordCommand request,
        CancellationToken cancellationToken
    )
    {
        var combinedWord = await wordGenerationService.GenerateWord(
            wordOne: request.WordOne,
            wordTwo: request.WordTwo
        );
        return combinedWord;
    }
}
