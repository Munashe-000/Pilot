using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Common.Interfaces;

public interface ITaskAssignmentRepository
{
    Task AddAsync(TaskAssignment task, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskAssignment>> ListByAssigneeRoleAsync(
        string tenantId,
        string assigneeRole,
        CancellationToken cancellationToken);
}
