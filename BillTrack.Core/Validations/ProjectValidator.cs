using BillTrack.Core.Contracts.Project;
using FluentValidation;

namespace BillTrack.Core.Validations;

public class ProjectValidator : AbstractValidator<ProjectRequest>
{
    public ProjectValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name can't be empty")
            .MaximumLength(100).WithMessage("Length must be less than 100");
    }
}