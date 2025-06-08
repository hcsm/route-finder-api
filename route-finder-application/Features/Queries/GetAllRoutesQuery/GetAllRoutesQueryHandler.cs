using MediatR;
using Route.Finder.Domain.Contracts.Interfaces;

namespace Route.Finder.Application.Features.Queries.GetAllRoutesQuery
{
    public class GetAllRoutesQueryHandler : IRequestHandler<GetAllRoutesQuery, IEnumerable<GetAllRoutesQueryResponse>>
    {
        private readonly IRouteRepository _routeRepository;

        public GetAllRoutesQueryHandler(
            IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<IEnumerable<GetAllRoutesQueryResponse>> Handle(
            GetAllRoutesQuery request, 
            CancellationToken cancellationToken)
        {
            var routes = await _routeRepository.GetAllAsync();

            return routes.Select(r => new GetAllRoutesQueryResponse(r.Id, r.Origin, r.Destination, r.Cost));
        }
    }
}
