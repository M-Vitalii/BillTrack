using BillTrack.Core.Contracts.Project;
using FastEndpoints;

namespace BillTrack.Api.Validators.Project;

public class ProjectUpdateValidator : Validator<ProjectUpdateRequest>
{
    public ProjectUpdateValidator()
    {
        RuleFor(i => i.Id)
            .NotEmpty().WithMessage("Id can't be empty");
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name can't be empty")
            .MaximumLength(100).WithMessage("Length must be less than 100");
    }
}