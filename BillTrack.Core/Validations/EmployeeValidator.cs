using BillTrack.Core.Contracts.Employee;
using FluentValidation;

namespace BillTrack.Core.Validations;

public class EmployeeValidator : AbstractValidator<EmployeeRequest>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.Email)
            .EmailAddress().WithMessage("Incorrect email address");

        RuleFor(e => e.Firstname)
            .NotEmpty().WithMessage("Firstname can't be empty")
            .MaximumLength(100).WithMessage("Firstname must be less than 100");
        
        RuleFor(e => e.Lastname)
            .NotEmpty().WithMessage("Firstname can't be empty")
            .MaximumLength(100).WithMessage("Firstname must be less than 100");

        RuleFor(e => e.Salary)
            .NotNull().WithMessage("Salary can't be null")
            .GreaterThanOrEqualTo(0).WithMessage("Salary must be greater or equal to 0");
        
        RuleFor(i => i.ProjectId)
            .NotNull().WithMessage("Project id can't be null");
        
        RuleFor(i => i.DepartmentId)
            .NotNull().WithMessage("Department id can't be null");
    }
}