using Moq;
using Route.Finder.Application.Features.Queries.GetBestRouteQuery;
using Route.Finder.Domain.Contracts.Interfaces;
using Shouldly;
using Xunit;

namespace Route.Finder.Application.Tests.Queries
{
    public class GetBestRouteQueryHandlerTests : IDisposable
    {

        private readonly Mock<IRouteRepository> _routeRepository;
        private readonly GetBestRouteQueryHandler _handler;

        public GetBestRouteQueryHandlerTests() 
        {
            _routeRepository = new Mock<IRouteRepository>(MockBehavior.Strict);
            _handler = new(_routeRepository.Object);
        }

        [Fact]
        public async Task Should_return_cheapest_route()
        {
            // Arrange
            var routes = new List<Route.Finder.Domain.Entities.Route>
            {
                new(Guid.NewGuid(), "GRU", "BRC", 10),
                new(Guid.NewGuid(), "BRC", "SCL", 5),
                new(Guid.NewGuid(), "GRU", "CDG", 75),
                new(Guid.NewGuid(), "GRU", "SCL", 20),
                new(Guid.NewGuid(), "GRU", "ORL", 56),
                new(Guid.NewGuid(), "ORL", "CDG", 5),
                new(Guid.NewGuid(), "SCL", "ORL", 20)
            };

            var query = new GetBestRouteQuery("GRU", "CDG");
            _routeRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(routes.AsEnumerable()));


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.TotalCost.ShouldBe(40);
            result.Path.ShouldBe(new List<string> { "GRU", "BRC", "SCL", "ORL", "CDG" });
            _routeRepository.Verify(x => x.GetAllAsync(), Times.Once);
            _routeRepository.VerifyNoOtherCalls();
        }

        public void Dispose()
        {
            _routeRepository.VerifyAll();
            GC.SuppressFinalize(this);
        }
    }
}
