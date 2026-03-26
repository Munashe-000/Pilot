namespace PilotFlow.Contracts.Responses;

public sealed record TaskInboxItemResponse(
    Guid Id,
    Guid RequestId,
    string Title,
    string RequesterName,
    string SystemName,
    string AssignedToRole,
    string Status,
    string Priority,
    DateTime CreatedAtUtc,
    DateTime DueAtUtc);
