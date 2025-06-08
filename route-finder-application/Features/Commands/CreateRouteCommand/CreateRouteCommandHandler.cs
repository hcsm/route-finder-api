using MediatR;
using Route.Finder.Domain.Contracts.Interfaces;

namespace Route.Finder.Application.Features.Commands.CreateRouteCommand
{
    public class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, CreateRouteCommandResponse>
    {
        private readonly IRouteRepository _repository;

        public CreateRouteCommandHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateRouteCommandResponse> Handle(
            CreateRouteCommand request, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Origin))
                throw new ArgumentException("Origin is required.");

            if (string.IsNullOrWhiteSpace(request.Destination))
                throw new ArgumentException("Destination is required.");

            if (request.Cost <= 0)
                throw new ArgumentException("Cost must be greater than 0.");

            var route = new Route.Finder.Domain.Entities.Route(
                Guid.NewGuid(),
                request.Origin,
                request.Destination,
                request.Cost
            );

            await _repository.AddAsync(route);

            return new CreateRouteCommandResponse(route.Id, "Route created successfully.");
        }
    }
}
