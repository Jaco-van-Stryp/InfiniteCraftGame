using MediatR;

namespace InfiniteCraftGame.Features.CombineWord;

public readonly record struct CombineWordCommand(string WordOne, string WordTwo, Guid UserId)
    : IRequest<CombineWordResponse>;
