namespace PilotFlow.Contracts.Requests;

public sealed record DecideTaskRequest(
    string TenantId,
    string Decision,
    string DecidedBy,
    string? Comment);
