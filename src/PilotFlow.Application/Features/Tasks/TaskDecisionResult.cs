using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Features.Tasks;

public sealed record TaskDecisionResult(
    TaskDecisionOutcome Outcome,
    Guid? TaskId = null,
    Guid? RequestId = null,
    TaskStatus? TaskStatus = null,
    AccessRequestStatus? RequestStatus = null,
    TaskDecision? Decision = null,
    DateTime? DecidedAtUtc = null)
{
    public static TaskDecisionResult NotFound() => new(TaskDecisionOutcome.NotFound);

    public static TaskDecisionResult AlreadyCompleted(
        Guid taskId,
        Guid requestId,
        TaskStatus taskStatus,
        AccessRequestStatus requestStatus)
        => new(TaskDecisionOutcome.AlreadyCompleted, taskId, requestId, taskStatus, requestStatus);

    public static TaskDecisionResult Success(
        Guid taskId,
        Guid requestId,
        TaskStatus taskStatus,
        AccessRequestStatus requestStatus,
        TaskDecision decision,
        DateTime decidedAtUtc)
        => new(TaskDecisionOutcome.Success, taskId, requestId, taskStatus, requestStatus, decision, decidedAtUtc);
}
