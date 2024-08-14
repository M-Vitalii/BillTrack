using BillTrack.Core.Contracts.Workday;
using FluentValidation;

namespace BillTrack.Core.Validations;

public class WorkdayValidator : AbstractValidator<WorkdayRequest>
{
    public WorkdayValidator()
    {
        RuleFor(w => w.Date)
            .NotNull().WithMessage("Date can't be null")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date cannot be in the future.");
        
        RuleFor(w => w.Hours)
            .NotNull().WithMessage("Hours can't be null");
        
        RuleFor(w => w.EmployeeId)
            .NotNull().WithMessage("Employee id can't be null");
    }
}