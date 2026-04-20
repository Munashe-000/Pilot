using MediatR;
using PilotFlow.Application.Features.Audit;
using PilotFlow.Contracts.Responses;

namespace PilotFlow.Api.Endpoints;

public static class AuditEndpoints
{
    public static RouteGroupBuilder MapAuditEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/audit");

        group.MapGet("/timeline", async (
            string tenantId,
            int? limit,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return Results.BadRequest(new
                {
                    message = "tenantId is required."
                });
            }

            var query = new GetAuditTimelineQuery(tenantId, limit);
            var result = await mediator.Send(query, cancellationToken);

            var response = result
                .Select(item => new AuditTimelineItemResponse(
                    item.Id,
                    item.TaskId,
                    item.RequestId,
                    item.Decision.ToString(),
                    item.Actor,
                    item.Comment,
                    item.OccurredAtUtc))
                .ToList();

            return Results.Ok(response);
        });

        return group;
    }
}
