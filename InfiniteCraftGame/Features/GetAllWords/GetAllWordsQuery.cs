using MediatR;

namespace InfiniteCraftGame.Features.GetAllWords;

public readonly record struct GetAllWordsQuery(Guid? UserId) : IRequest<List<GetAllWordsResponse>>;
