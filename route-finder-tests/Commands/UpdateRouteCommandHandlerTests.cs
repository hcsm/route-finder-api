using Moq;
using Route.Finder.Application.Features.Commands.UpdateRouteCommand;
using Route.Finder.Domain.Contracts.Interfaces;
using Shouldly;
using Xunit;

namespace Route.Finder.Application.Tests.Commands
{
    public class UpdateRouteCommandHandlerTests : IDisposable
    {
        private readonly Mock<IRouteRepository> _routeRepository;
        private readonly UpdateRouteCommandHandler _handler;

        public UpdateRouteCommandHandlerTests()
        {
            _routeRepository = new Mock<IRouteRepository>(MockBehavior.Strict);
            _handler = new UpdateRouteCommandHandler(_routeRepository.Object);
        }

        [Fact]
        public async Task Should_update_existing_route_successfully()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var originBefore = "GRU";
            var destinationBefore = "MIA";
            var costBefore = 50m;

            var originAfter = "GRU";
            var destinationAfter = "CDG";
            var costAfter = 75m;

            var existingRoute = new Route.Finder.Domain.Entities.Route(routeId, originBefore, destinationBefore, costBefore);
            Route.Finder.Domain.Entities.Route? updatedRoute = null;

            _routeRepository
                .Setup(r => r.GetByIdAsync(routeId))
                .ReturnsAsync(existingRoute);

            _routeRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Route.Finder.Domain.Entities.Route>()))
                .Callback<Route.Finder.Domain.Entities.Route>(r => updatedRoute = r)
                .Returns(Task.CompletedTask);

            var command = new UpdateRouteCommand(routeId, originAfter, destinationAfter, costAfter);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(routeId);
            result.Message.ShouldBe("Route updated successfully.");

            updatedRoute.ShouldNotBeNull();
            updatedRoute!.Origin.ShouldBe(originAfter);
            updatedRoute.Destination.ShouldBe(destinationAfter);
            updatedRoute.Cost.ShouldBe(costAfter);

            _routeRepository.Verify(r => r.GetByIdAsync(routeId), Times.Once);
            _routeRepository.Verify(r => r.UpdateAsync(It.IsAny<Route.Finder.Domain.Entities.Route>()), Times.Once);
            _routeRepository.VerifyNoOtherCalls();
        }


        public void Dispose()
        {
            _routeRepository.VerifyAll();
            GC.SuppressFinalize(this);
        }
    }
}
