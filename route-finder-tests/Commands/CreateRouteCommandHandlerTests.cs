using Moq;
using Route.Finder.Application.Features.Commands.CreateRouteCommand;
using Route.Finder.Domain.Contracts.Interfaces;
using Shouldly;
using Xunit;

namespace Route.Finder.Application.Tests.Commands
{
    public class CreateRouteCommandHandlerTests : IDisposable
    {
        private readonly Mock<IRouteRepository> _routeRepository;
        private readonly CreateRouteCommandHandler _handler;

        public CreateRouteCommandHandlerTests()
        {
            _routeRepository = new Mock<IRouteRepository>(MockBehavior.Strict);
            _handler = new CreateRouteCommandHandler(_routeRepository.Object);
        }

        [Fact]
        public async Task Should_create_new_route_successfully()
        {
            // Arrange
            var command = new CreateRouteCommand("GRU", "MIA", 42);
            Route.Finder.Domain.Entities.Route? addedRoute = null;

            _routeRepository
                .Setup(r => r.AddAsync(It.IsAny<Route.Finder.Domain.Entities.Route>()))
                .Callback<Route.Finder.Domain.Entities.Route>(r => addedRoute = r)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(addedRoute!.Id);
            result.Message.ShouldBe("Route created successfully.");

            addedRoute.Origin.ShouldBe("GRU");
            addedRoute.Destination.ShouldBe("MIA");
            addedRoute.Cost.ShouldBe(42);

            _routeRepository.Verify(r => r.AddAsync(It.IsAny<Route.Finder.Domain.Entities.Route>()), Times.Once);
            _routeRepository.VerifyNoOtherCalls();
        }

        public void Dispose()
        {
            _routeRepository.VerifyAll();
            GC.SuppressFinalize(this);
        }
    }
}
