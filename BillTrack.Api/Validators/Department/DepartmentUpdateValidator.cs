using BillTrack.Core.Contracts.Department;
using FastEndpoints;

namespace BillTrack.Api.Validators.Department;

public class DepartmentUpdateValidator : Validator<DepartmentUpdateRequest>
{
    public DepartmentUpdateValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty().WithMessage("Id can't be empty");
        
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name can't be empty")
            .MaximumLength(100).WithMessage("Length must be less than 100");
    }
}