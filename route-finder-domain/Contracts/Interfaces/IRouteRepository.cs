namespace Route.Finder.Domain.Contracts.Interfaces
{
    public interface IRouteRepository
    {
        Task<IEnumerable<Route.Finder.Domain.Entities.Route>> GetAllAsync();
        Task<Route.Finder.Domain.Entities.Route?> GetByIdAsync(Guid id);
        Task AddAsync(Route.Finder.Domain.Entities.Route route);
        Task UpdateAsync(Route.Finder.Domain.Entities.Route route);
        Task DeleteAsync(Guid id);
    }
}
