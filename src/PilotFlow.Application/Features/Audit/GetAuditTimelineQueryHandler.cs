using MediatR;
using PilotFlow.Application.Common.Interfaces;

namespace PilotFlow.Application.Features.Audit;

public sealed class GetAuditTimelineQueryHandler
    : IRequestHandler<GetAuditTimelineQuery, IReadOnlyList<AuditTimelineItem>>
{
    private const int DefaultLimit = 25;
    private const int MaxLimit = 100;
    private readonly IAuditEventRepository _auditRepository;

    public GetAuditTimelineQueryHandler(IAuditEventRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public async Task<IReadOnlyList<AuditTimelineItem>> Handle(
        GetAuditTimelineQuery request,
        CancellationToken cancellationToken)
    {
        var events = await _auditRepository.ListByTenantAsync(request.TenantId, cancellationToken);
        var limit = request.Limit.HasValue && request.Limit.Value > 0
            ? Math.Min(request.Limit.Value, MaxLimit)
            : DefaultLimit;

        return events
            .OrderByDescending(auditEvent => auditEvent.OccurredAtUtc)
            .Take(limit)
            .Select(auditEvent => new AuditTimelineItem(
                auditEvent.Id,
                auditEvent.TaskId,
                auditEvent.RequestId,
                auditEvent.Decision,
                auditEvent.Actor,
                auditEvent.Comment,
                auditEvent.OccurredAtUtc))
            .ToList();
    }
}
