using MediatR;
using Route.Finder.Domain.Contracts.Interfaces;

namespace Route.Finder.Application.Features.Commands.UpdateRouteCommand
{
    public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand, UpdateRouteCommandResponse>
    {
        private readonly IRouteRepository _routeRepository;

        public UpdateRouteCommandHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<UpdateRouteCommandResponse> Handle(
            UpdateRouteCommand request, 
            CancellationToken cancellationToken)
        {
            var existingRoute = await _routeRepository.GetByIdAsync(request.Id);

            if (existingRoute == null)
            {
                throw new KeyNotFoundException($"Route with ID '{request.Id}' not found.");
            }

            var updatedRoute = existingRoute with
            {
                Origin = request.Origin,
                Destination = request.Destination,
                Cost = request.Cost
            };

            await _routeRepository.UpdateAsync(updatedRoute);

            return new UpdateRouteCommandResponse(updatedRoute.Id, "Route updated successfully.");
        }
    }
}
