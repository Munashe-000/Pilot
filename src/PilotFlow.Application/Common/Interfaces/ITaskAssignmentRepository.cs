using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Common.Interfaces;

public interface ITaskAssignmentRepository
{
    Task AddAsync(TaskAssignment task, CancellationToken cancellationToken);
    Task<TaskAssignment?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(TaskAssignment task, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskAssignment>> ListByAssigneeRoleAsync(
        string tenantId,
        string assigneeRole,
        CancellationToken cancellationToken);
}
