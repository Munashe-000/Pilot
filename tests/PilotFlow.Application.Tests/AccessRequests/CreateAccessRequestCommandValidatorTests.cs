using PilotFlow.Application.Features.AccessRequests;

namespace PilotFlow.Application.Tests.AccessRequests;

public sealed class CreateAccessRequestCommandValidatorTests
{
    [Fact]
    public void Validator_rejects_missing_fields()
    {
        var validator = new CreateAccessRequestCommandValidator();
        var command = new CreateAccessRequestCommand(
            string.Empty,
            string.Empty,
            "not-an-email",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 5);
    }

    [Fact]
    public void Validator_accepts_valid_payload()
    {
        var validator = new CreateAccessRequestCommandValidator();
        var command = new CreateAccessRequestCommand(
            "tenant-demo",
            "Refilwe Peters",
            "refilwe@company.com",
            "Analytics Hub",
            "Read",
            "Need to review dashboards.",
            "Sipho Dlamini");

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }
}
