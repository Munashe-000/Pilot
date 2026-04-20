using PilotFlow.Application.Features.Audit;

namespace PilotFlow.Application.Tests.Audit;

public sealed class GetAuditTimelineQueryValidatorTests
{
    [Fact]
    public void Validator_rejects_missing_tenant_id()
    {
        var validator = new GetAuditTimelineQueryValidator();
        var query = new GetAuditTimelineQuery(string.Empty, 25);

        var result = validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "TenantId");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Validator_rejects_limit_outside_allowed_range(int limit)
    {
        var validator = new GetAuditTimelineQueryValidator();
        var query = new GetAuditTimelineQuery("tenant-demo", limit);

        var result = validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Limit");
    }

    [Fact]
    public void Validator_accepts_null_limit()
    {
        var validator = new GetAuditTimelineQueryValidator();
        var query = new GetAuditTimelineQuery("tenant-demo", null);

        var result = validator.Validate(query);

        Assert.True(result.IsValid);
    }
}
