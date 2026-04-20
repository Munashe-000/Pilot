using PilotFlow.Application.Features.Tasks;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Tests.Tasks;

public sealed class DecideTaskCommandValidatorTests
{
    [Fact]
    public void Validator_rejects_missing_required_fields()
    {
        var validator = new DecideTaskCommandValidator();
        var command = new DecideTaskCommand(
            string.Empty,
            Guid.Empty,
            TaskDecision.Approved,
            string.Empty,
            null);

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "TenantId");
        Assert.Contains(result.Errors, error => error.PropertyName == "TaskId");
        Assert.Contains(result.Errors, error => error.PropertyName == "DecidedBy");
    }

    [Fact]
    public void Validator_rejects_comment_that_is_too_long()
    {
        var validator = new DecideTaskCommandValidator();
        var command = new DecideTaskCommand(
            "tenant-demo",
            Guid.NewGuid(),
            TaskDecision.Rejected,
            "Security Lead",
            new string('x', 501));

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Comment");
    }

    [Fact]
    public void Validator_accepts_valid_payload()
    {
        var validator = new DecideTaskCommandValidator();
        var command = new DecideTaskCommand(
            "tenant-demo",
            Guid.NewGuid(),
            TaskDecision.Approved,
            "Security Lead",
            "Approved after review.");

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }
}
