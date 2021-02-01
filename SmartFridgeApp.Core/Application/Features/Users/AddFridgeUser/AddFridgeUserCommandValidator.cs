using FluentValidation;
using System;

namespace SmartFridgeApp.Core.Application.Features.Users.AddFridgeUser
{

    public class AddFridgeUserCommandValidator : AbstractValidator<AddFridgeUserCommand>
    {
        public AddFridgeUserCommandValidator()
        {
            RuleFor(command => command.User.Email)
                .NotNull()
                .MinimumLength(2)
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .MaximumLength(70)
                .WithMessage("Name must have minimum 2 characters and maximum 50 characters.");
        }
    }
}
