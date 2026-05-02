using MediatR;

namespace InfiniteCraftGame.Features.CombineWord;

public record CombineWordCommand(string WordOne, string WordTwo) : IRequest<CombineWordResponse>;
