using MediatR;
using Route.Finder.Domain.Contracts.Interfaces;

namespace Route.Finder.Application.Features.Commands.DeleteRouteCommand
{
    public class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand, DeleteRouteCommandResponse>
    {
        private readonly IRouteRepository _routeRepository;

        public DeleteRouteCommandHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<DeleteRouteCommandResponse> Handle(
            DeleteRouteCommand request, 
            CancellationToken cancellationToken)
        {
            var existing = await _routeRepository.GetByIdAsync(request.Id);

            if (existing == null)
                throw new KeyNotFoundException($"Route with ID '{request.Id}' not found.");

            await _routeRepository.DeleteAsync(request.Id);
            return new DeleteRouteCommandResponse(request.Id, "Route deleted successfully.");
        }
    }
}
