using Moq;
using Route.Finder.Application.Features.Queries.GetAllRoutesQuery;
using Route.Finder.Domain.Contracts.Interfaces;
using Shouldly;
using Xunit;

namespace Route.Finder.Application.Tests.Queries
{
    public class GetAllRoutesQueryHandlerTests : IDisposable
    {
        private readonly Mock<IRouteRepository> _routeRepository;
        private readonly GetAllRoutesQueryHandler _handler;

        public GetAllRoutesQueryHandlerTests()
        {
            _routeRepository = new Mock<IRouteRepository>(MockBehavior.Strict);
            _handler = new(_routeRepository.Object);
        }

        [Fact]
        public async Task Should_return_all_routes()
        {
            // Arrange
            var routes = new List<Route.Finder.Domain.Entities.Route>
            {
                new(Guid.NewGuid(), "GRU", "CDG", 75),
                new(Guid.NewGuid(), "GRU", "BRC", 10),
            };
           
            var query = new GetAllRoutesQuery();
            _routeRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(routes.AsEnumerable()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);

            result.ShouldContain(r => r.Origin == "GRU" && r.Destination == "CDG" && r.Cost == 75);
            result.ShouldContain(r => r.Origin == "GRU" && r.Destination == "BRC" && r.Cost == 10);

            _routeRepository.Verify(r => r.GetAllAsync(), Times.Once);
            _routeRepository.VerifyNoOtherCalls();
        }

        public void Dispose()
        {
            _routeRepository.VerifyAll();
            GC.SuppressFinalize(this);
        }
    }
}
