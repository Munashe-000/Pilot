using MediatR;
using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Features.Tasks;

public sealed class DecideTaskCommandHandler
    : IRequestHandler<DecideTaskCommand, TaskDecisionResult>
{
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly ITaskAssignmentRepository _taskRepository;
    private readonly IAuditEventRepository _auditRepository;
    private readonly IDateTimeProvider _clock;

    public DecideTaskCommandHandler(
        IAccessRequestRepository accessRequestRepository,
        ITaskAssignmentRepository taskRepository,
        IAuditEventRepository auditRepository,
        IDateTimeProvider clock)
    {
        _accessRequestRepository = accessRequestRepository;
        _taskRepository = taskRepository;
        _auditRepository = auditRepository;
        _clock = clock;
    }

    public async Task<TaskDecisionResult> Handle(
        DecideTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(
            request.TenantId,
            request.TaskId,
            cancellationToken);

        if (task is null)
        {
            return TaskDecisionResult.NotFound();
        }

        var accessRequest = await _accessRequestRepository.GetByIdAsync(
            request.TenantId,
            task.RequestId,
            cancellationToken);

        if (accessRequest is null)
        {
            return TaskDecisionResult.NotFound();
        }

        if (task.Status == TaskStatus.Completed &&
            (accessRequest.Status == AccessRequestStatus.Approved ||
             accessRequest.Status == AccessRequestStatus.Rejected))
        {
            return TaskDecisionResult.AlreadyCompleted(
                task.Id,
                task.RequestId,
                task.Status,
                accessRequest.Status);
        }

        var now = _clock.UtcNow;

        if (request.Decision == TaskDecision.Approved)
        {
            accessRequest.MarkApproved();
        }
        else
        {
            accessRequest.MarkRejected();
        }

        task.MarkCompleted();

        await _taskRepository.UpdateAsync(task, cancellationToken);
        await _accessRequestRepository.UpdateAsync(accessRequest, cancellationToken);

        var auditEvent = AuditEvent.Record(
            Guid.NewGuid(),
            request.TenantId,
            task.Id,
            task.RequestId,
            request.Decision,
            request.DecidedBy,
            request.Comment,
            now);

        await _auditRepository.AddAsync(auditEvent, cancellationToken);

        return TaskDecisionResult.Success(
            task.Id,
            task.RequestId,
            task.Status,
            accessRequest.Status,
            request.Decision,
            now);
    }
}
