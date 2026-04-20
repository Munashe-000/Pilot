using PilotFlow.Domain.Entities;
using DomainTaskStatus = PilotFlow.Domain.Entities.TaskStatus;

namespace PilotFlow.Application.Features.Tasks;

public sealed record TaskDecisionResult(
    TaskDecisionOutcome Outcome,
    Guid? TaskId = null,
    Guid? RequestId = null,
    DomainTaskStatus? TaskStatus = null,
    AccessRequestStatus? RequestStatus = null,
    TaskDecision? Decision = null,
    DateTime? DecidedAtUtc = null)
{
    public static TaskDecisionResult NotFound() => new(TaskDecisionOutcome.NotFound);

    public static TaskDecisionResult AlreadyCompleted(
        Guid taskId,
        Guid requestId,
        DomainTaskStatus taskStatus,
        AccessRequestStatus requestStatus)
        => new(TaskDecisionOutcome.AlreadyCompleted, taskId, requestId, taskStatus, requestStatus);

    public static TaskDecisionResult Success(
        Guid taskId,
        Guid requestId,
        DomainTaskStatus taskStatus,
        AccessRequestStatus requestStatus,
        TaskDecision decision,
        DateTime decidedAtUtc)
        => new(TaskDecisionOutcome.Success, taskId, requestId, taskStatus, requestStatus, decision, decidedAtUtc);
}
