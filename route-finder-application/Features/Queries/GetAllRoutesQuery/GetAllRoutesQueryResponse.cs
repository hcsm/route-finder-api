namespace Route.Finder.Application.Features.Queries.GetAllRoutesQuery
{
    public record GetAllRoutesQueryResponse(
        Guid Id,
        string Origin,
        string Destination,
        decimal Cost
    );
}
