using FluentValidation;
using JobTracking.Application.DTOs.Application;

namespace JobTracking.Application.Validators
{
    public class ApplicationCreateDtoValidator : AbstractValidator<ApplicationCreateDto>
    {
        public ApplicationCreateDtoValidator()
        {
            RuleFor(x => x.JobAdvertisementId)
                .GreaterThan(0)
                .WithMessage("ID на обявата е задължително.");
        }
    }
}
