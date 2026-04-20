namespace PilotFlow.Domain.Entities;

public sealed class AuditEvent
{
    public Guid Id { get; private set; }
    public string TenantId { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid RequestId { get; private set; }
    public TaskDecision Decision { get; private set; }
    public string Actor { get; private set; }
    public string? Comment { get; private set; }
    public DateTime OccurredAtUtc { get; private set; }

    private AuditEvent(
        Guid id,
        string tenantId,
        Guid taskId,
        Guid requestId,
        TaskDecision decision,
        string actor,
        string? comment,
        DateTime occurredAtUtc)
    {
        Id = id;
        TenantId = tenantId;
        TaskId = taskId;
        RequestId = requestId;
        Decision = decision;
        Actor = actor;
        Comment = comment;
        OccurredAtUtc = occurredAtUtc;
    }

    public static AuditEvent Record(
        Guid id,
        string tenantId,
        Guid taskId,
        Guid requestId,
        TaskDecision decision,
        string actor,
        string? comment,
        DateTime occurredAtUtc)
    {
        return new AuditEvent(id, tenantId, taskId, requestId, decision, actor, comment, occurredAtUtc);
    }
}
