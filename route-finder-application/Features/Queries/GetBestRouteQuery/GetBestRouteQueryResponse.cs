namespace Route.Finder.Application.Features.Queries.GetBestRouteQuery
{
    public record GetBestRouteQueryResponse(List<string> Path, decimal TotalCost);
}
