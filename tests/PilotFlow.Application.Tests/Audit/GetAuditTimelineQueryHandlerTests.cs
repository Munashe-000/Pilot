using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Application.Features.Audit;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Tests.Audit;

public sealed class GetAuditTimelineQueryHandlerTests
{
    [Fact]
    public async Task Handle_returns_most_recent_events_first()
    {
        var oldest = AuditEvent.Record(
            Guid.NewGuid(),
            "tenant-demo",
            Guid.NewGuid(),
            Guid.NewGuid(),
            TaskDecision.Approved,
            "Security Lead",
            "Oldest event.",
            new DateTime(2026, 2, 10, 8, 0, 0, DateTimeKind.Utc));
        var newest = AuditEvent.Record(
            Guid.NewGuid(),
            "tenant-demo",
            Guid.NewGuid(),
            Guid.NewGuid(),
            TaskDecision.Rejected,
            "Security Lead",
            "Newest event.",
            new DateTime(2026, 2, 10, 10, 0, 0, DateTimeKind.Utc));
        var middle = AuditEvent.Record(
            Guid.NewGuid(),
            "tenant-demo",
            Guid.NewGuid(),
            Guid.NewGuid(),
            TaskDecision.Approved,
            "Security Lead",
            "Middle event.",
            new DateTime(2026, 2, 10, 9, 0, 0, DateTimeKind.Utc));

        var repository = new FakeAuditEventRepository(oldest, newest, middle);
        var handler = new GetAuditTimelineQueryHandler(repository);

        var result = await handler.Handle(
            new GetAuditTimelineQuery("tenant-demo", null),
            CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal(newest.Id, result[0].Id);
        Assert.Equal(middle.Id, result[1].Id);
        Assert.Equal(oldest.Id, result[2].Id);
    }

    [Fact]
    public async Task Handle_honors_requested_limit()
    {
        var repository = new FakeAuditEventRepository(
            CreateEvent("tenant-demo", 8),
            CreateEvent("tenant-demo", 9),
            CreateEvent("tenant-demo", 10));
        var handler = new GetAuditTimelineQueryHandler(repository);

        var result = await handler.Handle(
            new GetAuditTimelineQuery("tenant-demo", 2),
            CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Handle_caps_limit_at_maximum()
    {
        var events = Enumerable.Range(1, 110)
            .Select(offset => CreateEvent("tenant-demo", offset))
            .ToArray();
        var repository = new FakeAuditEventRepository(events);
        var handler = new GetAuditTimelineQueryHandler(repository);

        var result = await handler.Handle(
            new GetAuditTimelineQuery("tenant-demo", 150),
            CancellationToken.None);

        Assert.Equal(100, result.Count);
    }

    [Fact]
    public async Task Handle_only_returns_events_for_requested_tenant()
    {
        var repository = new FakeAuditEventRepository(
            CreateEvent("tenant-demo", 1),
            CreateEvent("tenant-other", 2),
            CreateEvent("tenant-demo", 3));
        var handler = new GetAuditTimelineQueryHandler(repository);

        var result = await handler.Handle(
            new GetAuditTimelineQuery("tenant-demo", null),
            CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.Contains("tenant-demo", item.Actor));
    }

    private static AuditEvent CreateEvent(string tenantId, int hour)
    {
        return AuditEvent.Record(
            Guid.NewGuid(),
            tenantId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            TaskDecision.Approved,
            $"{tenantId}-actor",
            $"Event at {hour}.",
            new DateTime(2026, 2, 11, hour % 24, 0, 0, DateTimeKind.Utc));
    }

    private sealed class FakeAuditEventRepository : IAuditEventRepository
    {
        private readonly List<AuditEvent> _events;

        public FakeAuditEventRepository(params AuditEvent[] events)
        {
            _events = events.ToList();
        }

        public Task AddAsync(AuditEvent auditEvent, CancellationToken cancellationToken)
        {
            _events.Add(auditEvent);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<AuditEvent>> ListByTenantAsync(
            string tenantId,
            CancellationToken cancellationToken)
        {
            var events = _events
                .Where(item => item.TenantId == tenantId)
                .ToList();

            return Task.FromResult<IReadOnlyList<AuditEvent>>(events);
        }
    }
}
