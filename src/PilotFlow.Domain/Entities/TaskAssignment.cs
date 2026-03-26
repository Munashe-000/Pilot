namespace PilotFlow.Domain.Entities;

public sealed class TaskAssignment
{
    public Guid Id { get; private set; }
    public string TenantId { get; private set; }
    public Guid RequestId { get; private set; }
    public string Title { get; private set; }
    public string RequesterName { get; private set; }
    public string SystemName { get; private set; }
    public string AssignedToRole { get; private set; }
    public DateTime DueAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }

    private TaskAssignment(
        Guid id,
        string tenantId,
        Guid requestId,
        string title,
        string requesterName,
        string systemName,
        string assignedToRole,
        DateTime createdAtUtc,
        DateTime dueAtUtc,
        TaskStatus status,
        TaskPriority priority)
    {
        Id = id;
        TenantId = tenantId;
        RequestId = requestId;
        Title = title;
        RequesterName = requesterName;
        SystemName = systemName;
        AssignedToRole = assignedToRole;
        CreatedAtUtc = createdAtUtc;
        DueAtUtc = dueAtUtc;
        Status = status;
        Priority = priority;
    }

    public static TaskAssignment Create(
        Guid id,
        string tenantId,
        Guid requestId,
        string title,
        string requesterName,
        string systemName,
        string assignedToRole,
        DateTime createdAtUtc,
        DateTime dueAtUtc,
        TaskPriority priority)
    {
        return new TaskAssignment(
            id,
            tenantId,
            requestId,
            title,
            requesterName,
            systemName,
            assignedToRole,
            createdAtUtc,
            dueAtUtc,
            TaskStatus.Pending,
            priority);
    }

    public void MarkInProgress()
    {
        Status = TaskStatus.InProgress;
    }

    public void MarkCompleted()
    {
        Status = TaskStatus.Completed;
    }

    public void MarkEscalated()
    {
        Status = TaskStatus.Escalated;
    }
}
