using MediatR;

namespace PilotFlow.Application.Features.Audit;

public sealed record GetAuditTimelineQuery(
    string TenantId,
    int? Limit) : IRequest<IReadOnlyList<AuditTimelineItem>>;
