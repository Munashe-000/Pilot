using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PilotFlow.Application.Common.Behaviors;

namespace PilotFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));
        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
