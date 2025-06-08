namespace Route.Finder.Domain.Entities
{
    public record Route(Guid Id, string Origin, string Destination, decimal Cost);
}
