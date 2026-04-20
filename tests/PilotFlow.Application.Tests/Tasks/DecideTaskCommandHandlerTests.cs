using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Application.Features.Tasks;
using PilotFlow.Domain.Entities;
using DomainTaskStatus = PilotFlow.Domain.Entities.TaskStatus;

namespace PilotFlow.Application.Tests.Tasks;

public sealed class DecideTaskCommandHandlerTests
{
    [Fact]
    public async Task Handle_updates_task_request_and_audit()
    {
        var now = new DateTime(2026, 2, 2, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FixedDateTimeProvider(now);
        var accessRequest = AccessRequest.Create(
            Guid.NewGuid(),
            "tenant-demo",
            "Ada Lovelace",
            "ada@company.com",
            "Analytics Hub",
            "Read",
            "Need to review reports.",
            "Sipho Dlamini",
            now.AddHours(-2));
        var task = TaskAssignment.Create(
            Guid.NewGuid(),
            "tenant-demo",
            accessRequest.Id,
            "Access request: Analytics Hub",
            "Ada Lovelace",
            "Analytics Hub",
            "Security",
            now.AddHours(-2),
            now.AddHours(6),
            TaskPriority.Normal);

        var accessRepository = new FakeAccessRequestRepository(accessRequest);
        var taskRepository = new FakeTaskAssignmentRepository(task);
        var auditRepository = new FakeAuditEventRepository();
        var handler = new DecideTaskCommandHandler(
            accessRepository,
            taskRepository,
            auditRepository,
            clock);

        var command = new DecideTaskCommand(
            "tenant-demo",
            task.Id,
            TaskDecision.Approved,
            "Security Lead",
            "Reviewed and approved.");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(TaskDecisionOutcome.Success, result.Outcome);
        Assert.Equal(DomainTaskStatus.Completed, task.Status);
        Assert.Equal(AccessRequestStatus.Approved, accessRequest.Status);
        Assert.Single(auditRepository.Events);

        var audit = auditRepository.Events[0];
        Assert.Equal(task.Id, audit.TaskId);
        Assert.Equal(accessRequest.Id, audit.RequestId);
        Assert.Equal(TaskDecision.Approved, audit.Decision);
        Assert.Equal("Security Lead", audit.Actor);
        Assert.Equal("Reviewed and approved.", audit.Comment);
        Assert.Equal(now, audit.OccurredAtUtc);
    }

    [Fact]
    public async Task Handle_returns_conflict_when_task_completed()
    {
        var now = new DateTime(2026, 2, 3, 9, 30, 0, DateTimeKind.Utc);
        var clock = new FixedDateTimeProvider(now);
        var accessRequest = AccessRequest.Create(
            Guid.NewGuid(),
            "tenant-demo",
            "Nia Long",
            "nia@company.com",
            "Finance Vault",
            "Read",
            "Monthly close oversight.",
            "Lerato Molefe",
            now.AddHours(-1));
        accessRequest.MarkApproved();
        var task = TaskAssignment.Create(
            Guid.NewGuid(),
            "tenant-demo",
            accessRequest.Id,
            "Access request: Finance Vault",
            "Nia Long",
            "Finance Vault",
            "Security",
            now.AddHours(-1),
            now.AddHours(4),
            TaskPriority.High);
        task.MarkCompleted();

        var accessRepository = new FakeAccessRequestRepository(accessRequest);
        var taskRepository = new FakeTaskAssignmentRepository(task);
        var auditRepository = new FakeAuditEventRepository();
        var handler = new DecideTaskCommandHandler(
            accessRepository,
            taskRepository,
            auditRepository,
            clock);

        var command = new DecideTaskCommand(
            "tenant-demo",
            task.Id,
            TaskDecision.Rejected,
            "Security Lead",
            null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(TaskDecisionOutcome.AlreadyCompleted, result.Outcome);
        Assert.Empty(auditRepository.Events);
    }

    [Fact]
    public async Task Handle_returns_not_found_when_task_missing()
    {
        var clock = new FixedDateTimeProvider(new DateTime(2026, 2, 4, 8, 0, 0, DateTimeKind.Utc));
        var accessRepository = new FakeAccessRequestRepository(null);
        var taskRepository = new FakeTaskAssignmentRepository(null);
        var auditRepository = new FakeAuditEventRepository();
        var handler = new DecideTaskCommandHandler(
            accessRepository,
            taskRepository,
            auditRepository,
            clock);

        var command = new DecideTaskCommand(
            "tenant-demo",
            Guid.NewGuid(),
            TaskDecision.Approved,
            "Security Lead",
            null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(TaskDecisionOutcome.NotFound, result.Outcome);
    }

    private sealed class FakeAccessRequestRepository : IAccessRequestRepository
    {
        private readonly List<AccessRequest> _requests = new();

        public FakeAccessRequestRepository(AccessRequest? request)
        {
            if (request != null)
            {
                _requests.Add(request);
            }
        }

        public Task AddAsync(AccessRequest request, CancellationToken cancellationToken)
        {
            _requests.Add(request);
            return Task.CompletedTask;
        }

        public Task<AccessRequest?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken)
        {
            var request = _requests.FirstOrDefault(item => item.TenantId == tenantId && item.Id == id);
            return Task.FromResult<AccessRequest?>(request);
        }

        public Task UpdateAsync(AccessRequest request, CancellationToken cancellationToken)
        {
            var index = _requests.FindIndex(item => item.Id == request.Id);
            if (index >= 0)
            {
                _requests[index] = request;
            }
            else
            {
                _requests.Add(request);
            }

            return Task.CompletedTask;
        }
    }

    private sealed class FakeTaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly List<TaskAssignment> _tasks = new();

        public FakeTaskAssignmentRepository(TaskAssignment? task)
        {
            if (task != null)
            {
                _tasks.Add(task);
            }
        }

        public Task AddAsync(TaskAssignment task, CancellationToken cancellationToken)
        {
            _tasks.Add(task);
            return Task.CompletedTask;
        }

        public Task<TaskAssignment?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken)
        {
            var task = _tasks.FirstOrDefault(item => item.TenantId == tenantId && item.Id == id);
            return Task.FromResult<TaskAssignment?>(task);
        }

        public Task UpdateAsync(TaskAssignment task, CancellationToken cancellationToken)
        {
            var index = _tasks.FindIndex(item => item.Id == task.Id);
            if (index >= 0)
            {
                _tasks[index] = task;
            }
            else
            {
                _tasks.Add(task);
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<TaskAssignment>> ListByAssigneeRoleAsync(
            string tenantId,
            string assigneeRole,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<TaskAssignment>>(_tasks);
        }
    }

    private sealed class FakeAuditEventRepository : IAuditEventRepository
    {
        public List<AuditEvent> Events { get; } = new();

        public Task AddAsync(AuditEvent auditEvent, CancellationToken cancellationToken)
        {
            Events.Add(auditEvent);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<AuditEvent>> ListByTenantAsync(
            string tenantId,
            CancellationToken cancellationToken)
        {
            var results = Events.Where(item => item.TenantId == tenantId).ToList();
            return Task.FromResult<IReadOnlyList<AuditEvent>>(results);
        }
    }

    private sealed class FixedDateTimeProvider : IDateTimeProvider
    {
        public FixedDateTimeProvider(DateTime utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTime UtcNow { get; }
    }
}
