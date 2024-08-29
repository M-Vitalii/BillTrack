using BillTrack.Core.Contracts.Department;
using FastEndpoints;

namespace BillTrack.Api.Validators;

public class DepartmentValidator : Validator<DepartmentRequest>
{
    public DepartmentValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name can't be empty")
            .MaximumLength(100).WithMessage("Length must be less than 100");
    }
}