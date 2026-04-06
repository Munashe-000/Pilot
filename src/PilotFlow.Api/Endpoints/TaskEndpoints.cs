using MediatR;
using PilotFlow.Application.Features.Tasks;
using PilotFlow.Contracts.Requests;
using PilotFlow.Contracts.Responses;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Api.Endpoints;

public static class TaskEndpoints
{
    public static RouteGroupBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks");

        group.MapGet("/inbox", async (
            string tenantId,
            string assigneeRole,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(tenantId) || string.IsNullOrWhiteSpace(assigneeRole))
            {
                return Results.BadRequest(new
                {
                    message = "tenantId and assigneeRole are required."
                });
            }

            var query = new GetTaskInboxQuery(tenantId, assigneeRole);
            var result = await mediator.Send(query, cancellationToken);

            var response = result
                .Select(task => new TaskInboxItemResponse(
                    task.Id,
                    task.RequestId,
                    task.Title,
                    task.RequesterName,
                    task.SystemName,
                    task.AssignedToRole,
                    task.Status.ToString(),
                    task.Priority.ToString(),
                    task.CreatedAtUtc,
                    task.DueAtUtc))
                .ToList();

            return Results.Ok(response);
        });

        group.MapPost("/{taskId:guid}/decision", async (
            Guid taskId,
            DecideTaskRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (!Enum.TryParse<TaskDecision>(request.Decision, true, out var decision))
            {
                return Results.BadRequest(new
                {
                    message = "Decision must be Approved or Rejected."
                });
            }

            var command = new DecideTaskCommand(
                request.TenantId,
                taskId,
                decision,
                request.DecidedBy,
                request.Comment);

            var result = await mediator.Send(command, cancellationToken);

            return result.Outcome switch
            {
                TaskDecisionOutcome.NotFound => Results.NotFound(new
                {
                    message = "Task or access request not found."
                }),
                TaskDecisionOutcome.AlreadyCompleted => Results.Conflict(new
                {
                    message = "Task is already completed."
                }),
                TaskDecisionOutcome.Success => Results.Ok(new DecideTaskResponse(
                    result.TaskId!.Value,
                    result.RequestId!.Value,
                    result.TaskStatus!.Value.ToString(),
                    result.RequestStatus!.Value.ToString(),
                    result.Decision!.Value.ToString(),
                    result.DecidedAtUtc!.Value)),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
            };
        });

        return group;
    }
}
