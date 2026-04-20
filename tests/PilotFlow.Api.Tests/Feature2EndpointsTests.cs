using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PilotFlow.Contracts.Requests;

namespace PilotFlow.Api.Tests;

public sealed class Feature2EndpointsTests
{
    [Fact]
    public async Task Decide_task_records_decision_and_audit_event()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var createRequest = new CreateAccessRequestRequest(
            "tenant-demo",
            "Ada Lovelace",
            "ada@company.com",
            "Analytics Hub",
            "Read",
            "Need access for quarterly review.",
            "Sipho Dlamini");

        var createResponse = await client.PostAsJsonAsync("/api/access-requests", createRequest);
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateAccessRequestEnvelope>();
        Assert.NotNull(created);

        var decisionRequest = new DecideTaskRequest(
            "tenant-demo",
            "Approved",
            "Security Lead",
            "Approved after access review.");

        var decisionResponse = await client.PostAsJsonAsync(
            $"/api/tasks/{created.TaskId}/decision",
            decisionRequest);

        decisionResponse.EnsureSuccessStatusCode();

        var decision = await decisionResponse.Content.ReadFromJsonAsync<DecideTaskEnvelope>();
        Assert.NotNull(decision);
        Assert.Equal("Completed", decision.TaskStatus);
        Assert.Equal("Approved", decision.RequestStatus);
        Assert.Equal("Approved", decision.Decision);

        var auditResponse = await client.GetAsync("/api/audit/timeline?tenantId=tenant-demo&limit=10");
        auditResponse.EnsureSuccessStatusCode();

        var events = await auditResponse.Content.ReadFromJsonAsync<List<AuditTimelineEnvelope>>();
        Assert.NotNull(events);
        Assert.Single(events);
        Assert.Equal(created.RequestId, events[0].RequestId);
        Assert.Equal(created.TaskId, events[0].TaskId);
        Assert.Equal("Approved", events[0].Decision);
        Assert.Equal("Security Lead", events[0].Actor);
        Assert.Equal("Approved after access review.", events[0].Comment);
    }

    [Fact]
    public async Task Decide_task_rejects_invalid_decision_value()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var request = new DecideTaskRequest(
            "tenant-demo",
            "Escalated",
            "Security Lead",
            null);

        var response = await client.PostAsJsonAsync($"/api/tasks/{Guid.NewGuid()}/decision", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(
            "Decision must be Approved or Rejected.",
            payload.GetProperty("message").GetString());
    }

    [Fact]
    public async Task Audit_timeline_requires_tenant_id()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/audit/timeline");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadAsStringAsync();
        Assert.Contains("tenantId", payload);
    }

    private sealed record CreateAccessRequestEnvelope(
        Guid RequestId,
        Guid TaskId,
        string Status,
        DateTime CreatedAtUtc);

    private sealed record DecideTaskEnvelope(
        Guid TaskId,
        Guid RequestId,
        string TaskStatus,
        string RequestStatus,
        string Decision,
        DateTime DecidedAtUtc);

    private sealed record AuditTimelineEnvelope(
        Guid Id,
        Guid TaskId,
        Guid RequestId,
        string Decision,
        string Actor,
        string? Comment,
        DateTime OccurredAtUtc);
}
