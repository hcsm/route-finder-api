using Microsoft.Extensions.Configuration;
using Route.Finder.Domain.Contracts.Interfaces;
using System.Data;
using System.Text.Json;

namespace Router.Finder.Repository.Persistence
{
    public class RouteRepository : IRouteRepository
    {
        private readonly string _filePath;

        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public RouteRepository(IConfiguration configuration)
        {
            var relativePath = configuration["Storage:RouteFilePath"];
            _filePath = Path.Combine(AppContext.BaseDirectory, relativePath ?? "Data/routes.json");

        }

        public async Task<IEnumerable<Route.Finder.Domain.Entities.Route>> GetAllAsync()
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Route.Finder.Domain.Entities.Route>>(json, _serializerOptions) ?? new List<Route.Finder.Domain.Entities.Route>();
        }

        public async Task<Route.Finder.Domain.Entities.Route?> GetByIdAsync(Guid id)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(r => r.Id == id);
        }

        public async Task AddAsync(Route.Finder.Domain.Entities.Route route)
        {
            var all = (await GetAllAsync()).ToList();
            all.Add(route);
            await SaveAllAsync(all);
        }

        public async Task UpdateAsync(Route.Finder.Domain.Entities.Route route)
        {
            var all = (await GetAllAsync()).ToList();
            var index = all.FindIndex(r => r.Id == route.Id);
            if (index != -1)
            {
                all[index] = route;
                await SaveAllAsync(all);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var all = (await GetAllAsync()).ToList();
            all = all.Where(r => r.Id != id).ToList();
            await SaveAllAsync(all);
        }

        private async Task SaveAllAsync(List<Route.Finder.Domain.Entities.Route> routes)
        {
            var json = JsonSerializer.Serialize(routes, _serializerOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
