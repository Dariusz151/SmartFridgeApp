using FluentValidation;

namespace SmartFridgeApp.Core.Application.Features.Fridges.AddFridge
{
    public class AddFridgeCommandValidator : AbstractValidator<AddFridgeCommand>
    {
        public AddFridgeCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(50)
                .WithMessage("Name must have minimum 2 characters and maximum 50 characters.");

            RuleFor(command => command.Address)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithMessage("Address must have minimum 5 characters and maximum 50 characters.");
        }
    }
}
