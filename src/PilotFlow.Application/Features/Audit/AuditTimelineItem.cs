using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Features.Audit;

public sealed record AuditTimelineItem(
    Guid Id,
    Guid TaskId,
    Guid RequestId,
    TaskDecision Decision,
    string Actor,
    string? Comment,
    DateTime OccurredAtUtc);
