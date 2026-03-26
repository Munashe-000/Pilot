using MediatR;
using PilotFlow.Application.Common.Interfaces;

namespace PilotFlow.Application.Features.Tasks;

public sealed class GetTaskInboxQueryHandler
    : IRequestHandler<GetTaskInboxQuery, IReadOnlyList<TaskInboxItem>>
{
    private readonly ITaskAssignmentRepository _taskRepository;

    public GetTaskInboxQueryHandler(ITaskAssignmentRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IReadOnlyList<TaskInboxItem>> Handle(
        GetTaskInboxQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.ListByAssigneeRoleAsync(
            request.TenantId,
            request.AssigneeRole,
            cancellationToken);

        return tasks
            .OrderBy(t => t.DueAtUtc)
            .Select(task => new TaskInboxItem(
                task.Id,
                task.RequestId,
                task.Title,
                task.RequesterName,
                task.SystemName,
                task.AssignedToRole,
                task.Status,
                task.Priority,
                task.CreatedAtUtc,
                task.DueAtUtc))
            .ToList();
    }
}
