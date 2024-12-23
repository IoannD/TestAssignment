using FluentValidation;
using TestAssignment.Application.Dtos;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Application.Validators;

internal class UploadClinicalTrialValidator : AbstractValidator<CreateClinicalTrialDto>
{
    public UploadClinicalTrialValidator()
    {
        RuleFor(x => x.TrialId)
            .NotEmpty().WithMessage("TrialId is required.")
            .MaximumLength(100).WithMessage("TrialId name cannot exceed 100 characters.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("StartDate is required.");

        RuleFor(x => x)
            .Must(x => x.EndDate == null || x.EndDate >= x.StartDate)
            .WithMessage("End date cannot be less than start date.");

        RuleFor(x => x.Participants)
            .GreaterThan(0).WithMessage("Number of participants should be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is required." + string.Join(", ", Enum.GetNames(typeof(Status))))
            .NotNull().WithMessage("Status is required.");
    }
}