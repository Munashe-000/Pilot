namespace PilotFlow.Contracts.Responses;

public sealed record DecideTaskResponse(
    Guid TaskId,
    Guid RequestId,
    string TaskStatus,
    string RequestStatus,
    string Decision,
    DateTime DecidedAtUtc);
