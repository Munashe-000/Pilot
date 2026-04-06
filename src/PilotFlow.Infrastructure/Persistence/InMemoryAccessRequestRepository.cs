using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Domain.Entities;
using PilotFlow.Infrastructure.Persistence.InMemory;

namespace PilotFlow.Infrastructure.Persistence;

public sealed class InMemoryAccessRequestRepository : IAccessRequestRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryAccessRequestRepository(InMemoryDataStore store)
    {
        _store = store;
    }

    public Task AddAsync(AccessRequest request, CancellationToken cancellationToken)
    {
        var bucket = _store.AccessRequests.GetOrAdd(request.TenantId, _ => new());
        bucket[request.Id] = request;
        return Task.CompletedTask;
    }

    public Task<AccessRequest?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken)
    {
        if (_store.AccessRequests.TryGetValue(tenantId, out var bucket) && bucket.TryGetValue(id, out var request))
        {
            return Task.FromResult<AccessRequest?>(request);
        }

        return Task.FromResult<AccessRequest?>(null);
    }

    public Task UpdateAsync(AccessRequest request, CancellationToken cancellationToken)
    {
        var bucket = _store.AccessRequests.GetOrAdd(request.TenantId, _ => new());
        bucket[request.Id] = request;
        return Task.CompletedTask;
    }
}
