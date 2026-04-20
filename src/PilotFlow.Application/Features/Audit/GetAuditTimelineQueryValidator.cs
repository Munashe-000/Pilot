using FluentValidation;

namespace PilotFlow.Application.Features.Audit;

public sealed class GetAuditTimelineQueryValidator : AbstractValidator<GetAuditTimelineQuery>
{
    public GetAuditTimelineQueryValidator()
    {
        RuleFor(query => query.TenantId)
            .NotEmpty();

        RuleFor(query => query.Limit)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .When(query => query.Limit.HasValue);
    }
}
