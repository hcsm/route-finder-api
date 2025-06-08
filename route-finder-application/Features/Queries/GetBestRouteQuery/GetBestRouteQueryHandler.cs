using MediatR;
using Route.Finder.Domain.Contracts.Interfaces;

namespace Route.Finder.Application.Features.Queries.GetBestRouteQuery
{
    public class GetBestRouteQueryHandler : IRequestHandler<GetBestRouteQuery, GetBestRouteQueryResponse?>
    {
        private readonly IRouteRepository _repository;

        public GetBestRouteQueryHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetBestRouteQueryResponse?> Handle(GetBestRouteQuery request, CancellationToken cancellationToken)
        {
            var routes = (await _repository.GetAllAsync()).ToList();

            var graph = BuildGraph(routes);

            var best = FindCheapestPath(graph, request.Origin.ToUpper(), request.Destination.ToUpper());

            return best is null
                ? null
                : new GetBestRouteQueryResponse(best.Value.Path, best.Value.Cost);
        }

        /// <summary>
        /// Constrói o grafo de rotas onde cada origem aponta para uma lista de destinos com custo.
        /// </summary>
        private Dictionary<string, List<(string Destination, decimal Cost)>> BuildGraph(
            IEnumerable<Domain.Entities.Route> routes)
        {
            var graph = new Dictionary<string, List<(string, decimal)>>();

            foreach (var route in routes)
            {
                if (!graph.ContainsKey(route.Origin))
                    graph[route.Origin] = new();

                graph[route.Origin].Add((route.Destination, route.Cost));
            }

            return graph;
        }

        // <summary>
        /// Busca o caminho mais barato entre origem e destino usando algoritmo similar ao Dijkstra.
        /// Utiliza PriorityQueue (fila de prioridade por custo).
        /// Fonte:
        /// - https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        /// - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2
        /// </summary>
        /// <param name="graph">Grafo de rotas</param>
        /// <param name="start">Origem</param>
        /// <param name="end">Destino</param>
        /// <returns>Lista de paradas no caminho e custo total</returns>
        private (List<string> Path, decimal Cost)? FindCheapestPath(
            Dictionary<string, 
            List<(string, decimal)>> graph, 
            string start, string end)
        {
            var queue = new PriorityQueue<(string Node, List<string> Path, decimal Cost), decimal>();
            queue.Enqueue((start, new List<string> { start }, 0), 0);

            var visited = new Dictionary<string, decimal>();

            while (queue.Count > 0)
            {
                var (current, path, cost) = queue.Dequeue();

                if (visited.ContainsKey(current) && visited[current] <= cost)
                    continue;

                visited[current] = cost;

                if (current == end)
                    return (path, cost);

                if (!graph.ContainsKey(current))
                    continue;

                foreach (var (neighbor, weight) in graph[current])
                {
                    var newCost = cost + weight;
                    var newPath = new List<string>(path) { neighbor };

                    queue.Enqueue((neighbor, newPath, newCost), newCost);
                }
            }

            return null;
        }
    }
}
