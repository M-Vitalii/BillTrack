using BillTrack.Core.Contracts.Department;
using FluentValidation;

namespace BillTrack.Core.Validations;

public class DepartmentValidator : AbstractValidator<DepartmentRequest>
{
    public DepartmentValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name can't be empty")
            .MaximumLength(100).WithMessage("Length must be less than 100");;
    }
}