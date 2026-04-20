namespace PilotFlow.Contracts.Responses;

public sealed record AuditTimelineItemResponse(
    Guid Id,
    Guid TaskId,
    Guid RequestId,
    string Decision,
    string Actor,
    string? Comment,
    DateTime OccurredAtUtc);
