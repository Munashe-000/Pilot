using MediatR;
using PilotFlow.Application.Features.AccessRequests;
using PilotFlow.Contracts.Requests;
using PilotFlow.Contracts.Responses;

namespace PilotFlow.Api.Endpoints;

public static class AccessRequestEndpoints
{
    public static RouteGroupBuilder MapAccessRequestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/access-requests");

        group.MapPost("/", async (
            CreateAccessRequestRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateAccessRequestCommand(
                request.TenantId,
                request.RequesterName,
                request.RequesterEmail,
                request.SystemName,
                request.AccessLevel,
                request.Reason,
                request.ManagerName);

            var result = await mediator.Send(command, cancellationToken);

            var response = new CreateAccessRequestResponse(
                result.RequestId,
                result.TaskId,
                result.Status.ToString(),
                result.CreatedAtUtc);

            return Results.Created($"/api/access-requests/{result.RequestId}", response);
        });

        return group;
    }
}
