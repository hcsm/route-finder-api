using MediatR;

namespace Route.Finder.Application.Features.Commands.DeleteRouteCommand
{
    public record DeleteRouteCommand(Guid Id) : IRequest<DeleteRouteCommandResponse>;
}
