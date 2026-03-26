using MediatR;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Features.AccessRequests;

public sealed record CreateAccessRequestCommand(
    string TenantId,
    string RequesterName,
    string RequesterEmail,
    string SystemName,
    string AccessLevel,
    string Reason,
    string ManagerName) : IRequest<AccessRequestCreatedResult>;

public sealed record AccessRequestCreatedResult(
    Guid RequestId,
    Guid TaskId,
    AccessRequestStatus Status,
    DateTime CreatedAtUtc);
