using MediatR;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Features.Tasks;

public sealed record DecideTaskCommand(
    string TenantId,
    Guid TaskId,
    TaskDecision Decision,
    string DecidedBy,
    string? Comment) : IRequest<TaskDecisionResult>;
