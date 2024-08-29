using BillTrack.Core.Contracts.Project;
using FastEndpoints;

namespace BillTrack.Api.Validators;

public class ProjectValidator : Validator<ProjectRequest>
{
    public ProjectValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name can't be empty")
            .MaximumLength(100).WithMessage("Length must be less than 100");
    }
}