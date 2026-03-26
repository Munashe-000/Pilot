using FluentValidation;

namespace PilotFlow.Application.Features.AccessRequests;

public sealed class CreateAccessRequestCommandValidator : AbstractValidator<CreateAccessRequestCommand>
{
    public CreateAccessRequestCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.RequesterName).NotEmpty().MaximumLength(120);
        RuleFor(x => x.RequesterEmail).NotEmpty().EmailAddress().MaximumLength(160);
        RuleFor(x => x.SystemName).NotEmpty().MaximumLength(120);
        RuleFor(x => x.AccessLevel).NotEmpty().MaximumLength(60);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ManagerName).NotEmpty().MaximumLength(120);
    }
}
