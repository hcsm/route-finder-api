using MediatR;

namespace Route.Finder.Application.Features.Queries.GetAllRoutesQuery
{
    public record GetAllRoutesQuery() : IRequest<IEnumerable<GetAllRoutesQueryResponse>>;
}
