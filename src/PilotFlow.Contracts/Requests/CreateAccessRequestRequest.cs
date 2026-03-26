namespace PilotFlow.Contracts.Requests;

public sealed record CreateAccessRequestRequest(
    string TenantId,
    string RequesterName,
    string RequesterEmail,
    string SystemName,
    string AccessLevel,
    string Reason,
    string ManagerName);
