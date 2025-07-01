using FluentValidation;
using JobTracking.Application.DTOs.JobAdvertisement;

namespace JobTracking.Application.Validators
{
    public class JobAdvertisementUpdateDtoValidator : AbstractValidator<JobAdvertisementUpdateDto>
    {
        public JobAdvertisementUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("ID на обявата е невалидно.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Заглавието е задължително.")
                .Length(5, 100)
                .WithMessage("Заглавието трябва да бъде между 5 и 100 символа.");

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Името на компанията е задължително.")
                .Length(3, 100)
                .WithMessage("Името на компанията трябва да бъде между 3 и 100 символа.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Описанието е задължително.")
                .Length(10, 1000)
                .WithMessage("Описанието трябва да бъде между 10 и 1000 символа.");
        }
    }
}
