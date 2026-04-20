using PilotFlow.Application.Common.Interfaces;
using PilotFlow.Application.Features.AccessRequests;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Tests.AccessRequests;

public sealed class CreateAccessRequestCommandHandlerTests
{
    [Fact]
    public async Task Handle_creates_access_request_and_task()
    {
        var clock = new FixedDateTimeProvider(new DateTime(2026, 1, 12, 10, 0, 0, DateTimeKind.Utc));
        var accessRepository = new FakeAccessRequestRepository();
        var taskRepository = new FakeTaskAssignmentRepository();
        var handler = new CreateAccessRequestCommandHandler(accessRepository, taskRepository, clock);

        var command = new CreateAccessRequestCommand(
            "tenant-demo",
            "Refilwe Peters",
            "refilwe@company.com",
            "Analytics Hub",
            "Read",
            "Need to review campaign dashboards.",
            "Sipho Dlamini");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(AccessRequestStatus.Submitted, result.Status);
        Assert.Equal(clock.UtcNow, result.CreatedAtUtc);
        Assert.Single(taskRepository.Tasks);
        Assert.NotEqual(Guid.Empty, result.RequestId);
        Assert.NotEqual(Guid.Empty, result.TaskId);

        var task = taskRepository.Tasks[0];
        Assert.Equal(result.RequestId, task.RequestId);
        Assert.Equal("Security", task.AssignedToRole);
        Assert.Equal("tenant-demo", task.TenantId);
    }

    private sealed class FakeAccessRequestRepository : IAccessRequestRepository
    {
        public AccessRequest? LastAdded { get; private set; }

        public Task AddAsync(AccessRequest request, CancellationToken cancellationToken)
        {
            LastAdded = request;
            return Task.CompletedTask;
        }

        public Task<AccessRequest?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(LastAdded);
        }

        public Task UpdateAsync(AccessRequest request, CancellationToken cancellationToken)
        {
            LastAdded = request;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeTaskAssignmentRepository : ITaskAssignmentRepository
    {
        public List<TaskAssignment> Tasks { get; } = new();

        public Task AddAsync(TaskAssignment task, CancellationToken cancellationToken)
        {
            Tasks.Add(task);
            return Task.CompletedTask;
        }

        public Task<TaskAssignment?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken)
        {
            var task = Tasks.FirstOrDefault(item => item.TenantId == tenantId && item.Id == id);
            return Task.FromResult<TaskAssignment?>(task);
        }

        public Task UpdateAsync(TaskAssignment task, CancellationToken cancellationToken)
        {
            var index = Tasks.FindIndex(item => item.Id == task.Id);
            if (index >= 0)
            {
                Tasks[index] = task;
            }
            else
            {
                Tasks.Add(task);
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<TaskAssignment>> ListByAssigneeRoleAsync(
            string tenantId,
            string assigneeRole,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<TaskAssignment>>(Tasks);
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
