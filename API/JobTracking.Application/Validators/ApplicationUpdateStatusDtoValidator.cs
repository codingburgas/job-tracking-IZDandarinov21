using FluentValidation;
using JobTracking.Application.DTOs.Application;
using JobTracking.Domain.Enums;
using System; // За Enum.IsDefined

namespace JobTracking.Application.Validators
{
    public class ApplicationUpdateStatusDtoValidator : AbstractValidator<ApplicationUpdateStatusDto>
    {
        public ApplicationUpdateStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Невалиден статус на кандидатурата.")
                .Must(status => Enum.IsDefined(typeof(ApplicationStatus), status))
                .WithMessage("Невалиден статус на кандидатурата.");
        }
    }
}
