namespace PilotFlow.Contracts.Responses;

public sealed record CreateAccessRequestResponse(
    Guid RequestId,
    Guid TaskId,
    string Status,
    DateTime CreatedAtUtc);
