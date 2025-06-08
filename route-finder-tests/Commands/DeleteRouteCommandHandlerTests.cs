using Moq;
using Route.Finder.Application.Features.Commands.DeleteRouteCommand;
using Route.Finder.Domain.Contracts.Interfaces;
using Shouldly;
using Xunit;

namespace Route.Finder.Application.Tests.Commands
{
    public class DeleteRouteCommandHandlerTests : IDisposable
    {
        private readonly Mock<IRouteRepository> _routeRepository;
        private readonly DeleteRouteCommandHandler _handler;

        public DeleteRouteCommandHandlerTests()
        {
            _routeRepository = new Mock<IRouteRepository>(MockBehavior.Strict);
            _handler = new DeleteRouteCommandHandler(_routeRepository.Object);
        }

        [Fact]
        public async Task Should_delete_existing_route()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var existingRoute = new Route.Finder.Domain.Entities.Route(routeId, "GRU", "MIA", 42);

            _routeRepository
                .Setup(r => r.GetByIdAsync(routeId))
                .ReturnsAsync(existingRoute);

            _routeRepository
                .Setup(r => r.DeleteAsync(routeId))
                .Returns(Task.CompletedTask);

            var command = new DeleteRouteCommand(routeId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(routeId);
            result.Message.ShouldBe("Route deleted successfully.");

            _routeRepository.Verify(r => r.GetByIdAsync(routeId), Times.Once);
            _routeRepository.Verify(r => r.DeleteAsync(routeId), Times.Once);
            _routeRepository.VerifyNoOtherCalls();
        }

        public void Dispose()
        {
            _routeRepository.VerifyAll();
            GC.SuppressFinalize(this);
        }
    }
}
