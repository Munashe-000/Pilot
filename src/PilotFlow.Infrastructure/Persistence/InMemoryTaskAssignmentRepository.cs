using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Domain.Entities;
using PilotFlow.Infrastructure.Persistence.InMemory;

namespace PilotFlow.Infrastructure.Persistence;

public sealed class InMemoryTaskAssignmentRepository : ITaskAssignmentRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryTaskAssignmentRepository(InMemoryDataStore store)
    {
        _store = store;
    }

    public Task AddAsync(TaskAssignment task, CancellationToken cancellationToken)
    {
        var bucket = _store.TaskAssignments.GetOrAdd(task.TenantId, _ => new());
        bucket[task.Id] = task;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<TaskAssignment>> ListByAssigneeRoleAsync(
        string tenantId,
        string assigneeRole,
        CancellationToken cancellationToken)
    {
        if (!_store.TaskAssignments.TryGetValue(tenantId, out var bucket))
        {
            return Task.FromResult<IReadOnlyList<TaskAssignment>>(Array.Empty<TaskAssignment>());
        }

        var results = bucket.Values
            .Where(task => task.AssignedToRole.Equals(assigneeRole, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult<IReadOnlyList<TaskAssignment>>(results);
    }
}
