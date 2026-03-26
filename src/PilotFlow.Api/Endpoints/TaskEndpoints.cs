using MediatR;
using PilotFlow.Application.Features.Tasks;
using PilotFlow.Contracts.Responses;

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

        return group;
    }
}
