using Microsoft.Extensions.DependencyInjection;
using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Infrastructure.Persistence;
using PilotFlow.Infrastructure.Persistence.InMemory;
using PilotFlow.Infrastructure.Services;

namespace PilotFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryDataStore>();
        services.AddSingleton<IAccessRequestRepository, InMemoryAccessRequestRepository>();
        services.AddSingleton<ITaskAssignmentRepository, InMemoryTaskAssignmentRepository>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
