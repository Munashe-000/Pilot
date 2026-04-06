using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Domain.Entities;
using PilotFlow.Infrastructure.Persistence.InMemory;

namespace PilotFlow.Infrastructure.Persistence;

public sealed class InMemoryAuditEventRepository : IAuditEventRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryAuditEventRepository(InMemoryDataStore store)
    {
        _store = store;
    }

    public Task AddAsync(AuditEvent auditEvent, CancellationToken cancellationToken)
    {
        var bucket = _store.AuditEvents.GetOrAdd(auditEvent.TenantId, _ => new());
        bucket[auditEvent.Id] = auditEvent;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<AuditEvent>> ListByTenantAsync(
        string tenantId,
        CancellationToken cancellationToken)
    {
        if (!_store.AuditEvents.TryGetValue(tenantId, out var bucket))
        {
            return Task.FromResult<IReadOnlyList<AuditEvent>>(Array.Empty<AuditEvent>());
        }

        return Task.FromResult<IReadOnlyList<AuditEvent>>(bucket.Values.ToList());
    }
}
