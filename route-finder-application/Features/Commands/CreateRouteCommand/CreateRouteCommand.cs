using MediatR;

namespace Route.Finder.Application.Features.Commands.CreateRouteCommand
{
    public record CreateRouteCommand(string Origin, string Destination, decimal Cost) : IRequest<CreateRouteCommandResponse>;
}
