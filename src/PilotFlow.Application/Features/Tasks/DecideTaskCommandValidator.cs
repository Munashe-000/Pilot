using FluentValidation;

namespace PilotFlow.Application.Features.Tasks;

public sealed class DecideTaskCommandValidator : AbstractValidator<DecideTaskCommand>
{
    public DecideTaskCommandValidator()
    {
        RuleFor(command => command.TenantId)
            .NotEmpty();

        RuleFor(command => command.TaskId)
            .NotEmpty();

        RuleFor(command => command.Decision)
            .IsInEnum();

        RuleFor(command => command.DecidedBy)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Comment)
            .MaximumLength(500)
            .When(command => !string.IsNullOrWhiteSpace(command.Comment));
    }
}
