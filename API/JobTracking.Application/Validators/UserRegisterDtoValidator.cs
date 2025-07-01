using FluentValidation;
using JobTracking.Application.DTOs.User;

namespace JobTracking.Application.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Името е задължително.")
                .Length(2, 50)
                .WithMessage("Името трябва да бъде между 2 и 50 символа.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Фамилията е задължителна.")
                .Length(2, 50)
                .WithMessage("Фамилията трябва да бъде между 2 и 50 символа.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Потребителското име е задължително.")
                .Length(5, 30)
                .WithMessage("Потребителското име трябва да бъде между 5 и 30 символа.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Паролата е задължителна.")
                .MinimumLength(6)
                .WithMessage("Паролата трябва да е поне 6 символа.");
        }
    }
}
