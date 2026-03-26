namespace PilotFlow.Domain.Entities;

public sealed class AccessRequest
{
    public Guid Id { get; private set; }
    public string TenantId { get; private set; }
    public string RequesterName { get; private set; }
    public string RequesterEmail { get; private set; }
    public string SystemName { get; private set; }
    public string AccessLevel { get; private set; }
    public string Reason { get; private set; }
    public string ManagerName { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public AccessRequestStatus Status { get; private set; }

    private AccessRequest(
        Guid id,
        string tenantId,
        string requesterName,
        string requesterEmail,
        string systemName,
        string accessLevel,
        string reason,
        string managerName,
        DateTime createdAtUtc,
        AccessRequestStatus status)
    {
        Id = id;
        TenantId = tenantId;
        RequesterName = requesterName;
        RequesterEmail = requesterEmail;
        SystemName = systemName;
        AccessLevel = accessLevel;
        Reason = reason;
        ManagerName = managerName;
        CreatedAtUtc = createdAtUtc;
        Status = status;
    }

    public static AccessRequest Create(
        Guid id,
        string tenantId,
        string requesterName,
        string requesterEmail,
        string systemName,
        string accessLevel,
        string reason,
        string managerName,
        DateTime createdAtUtc)
    {
        return new AccessRequest(
            id,
            tenantId,
            requesterName,
            requesterEmail,
            systemName,
            accessLevel,
            reason,
            managerName,
            createdAtUtc,
            AccessRequestStatus.Submitted);
    }

    public void MarkInReview()
    {
        Status = AccessRequestStatus.InReview;
    }

    public void MarkApproved()
    {
        Status = AccessRequestStatus.Approved;
    }

    public void MarkRejected()
    {
        Status = AccessRequestStatus.Rejected;
    }
}
