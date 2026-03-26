using MediatR;
using PilotFlow.Domain.Entities;
using TaskStatus = PilotFlow.Domain.Entities.TaskStatus;

namespace PilotFlow.Application.Features.Tasks;

public sealed record GetTaskInboxQuery(
    string TenantId,
    string AssigneeRole) : IRequest<IReadOnlyList<TaskInboxItem>>;

public sealed record TaskInboxItem(
    Guid Id,
    Guid RequestId,
    string Title,
    string RequesterName,
    string SystemName,
    string AssignedToRole,
    TaskStatus Status,
    TaskPriority Priority,
    DateTime CreatedAtUtc,
    DateTime DueAtUtc);
