using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Route.Finder.Application.Features.Commands.CreateRouteCommand;
using Route.Finder.Application.Features.Commands.DeleteRouteCommand;
using Route.Finder.Application.Features.Commands.UpdateRouteCommand;
using Route.Finder.Application.Features.Queries.GetAllRoutesQuery;
using Route.Finder.Domain.Contracts.Interfaces;
using Router.Finder.Repository.Persistence;

namespace Router.Finder.Ioc.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection ConfigureDependencyInjection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateRouteCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllRoutesQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdateRouteCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DeleteRouteCommandHandler).Assembly));

            services.AddRepositories(configuration);

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IRouteRepository>(provider =>
                new RouteRepository(configuration)
            );

            return services;
        }
    }
}
