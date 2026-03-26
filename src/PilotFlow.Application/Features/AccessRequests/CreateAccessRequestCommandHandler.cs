using MediatR;
using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Features.AccessRequests;

public sealed class CreateAccessRequestCommandHandler
    : IRequestHandler<CreateAccessRequestCommand, AccessRequestCreatedResult>
{
    private const string DefaultAssignedRole = "Security";
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly ITaskAssignmentRepository _taskRepository;
    private readonly IDateTimeProvider _clock;

    public CreateAccessRequestCommandHandler(
        IAccessRequestRepository accessRequestRepository,
        ITaskAssignmentRepository taskRepository,
        IDateTimeProvider clock)
    {
        _accessRequestRepository = accessRequestRepository;
        _taskRepository = taskRepository;
        _clock = clock;
    }

    public async Task<AccessRequestCreatedResult> Handle(
        CreateAccessRequestCommand request,
        CancellationToken cancellationToken)
    {
        var now = _clock.UtcNow;
        var requestId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var accessRequest = AccessRequest.Create(
            requestId,
            request.TenantId,
            request.RequesterName,
            request.RequesterEmail,
            request.SystemName,
            request.AccessLevel,
            request.Reason,
            request.ManagerName,
            now);

        var task = TaskAssignment.Create(
            taskId,
            request.TenantId,
            requestId,
            $"Access request: {request.SystemName}",
            request.RequesterName,
            request.SystemName,
            DefaultAssignedRole,
            now,
            now.AddHours(8),
            TaskPriority.Normal);

        await _accessRequestRepository.AddAsync(accessRequest, cancellationToken);
        await _taskRepository.AddAsync(task, cancellationToken);

        return new AccessRequestCreatedResult(
            accessRequest.Id,
            task.Id,
            accessRequest.Status,
            accessRequest.CreatedAtUtc);
    }
}
