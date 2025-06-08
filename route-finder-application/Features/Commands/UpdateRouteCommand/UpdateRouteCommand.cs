using MediatR;

namespace Route.Finder.Application.Features.Commands.UpdateRouteCommand
{
    public record UpdateRouteCommand(Guid Id, string Origin, string Destination, decimal Cost) : IRequest<UpdateRouteCommandResponse>;
}
