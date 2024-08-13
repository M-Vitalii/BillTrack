using BillTrack.Core.Contracts.Invoice;
using FluentValidation;

namespace BillTrack.Core.Validations;

public class InvoiceValidator : AbstractValidator<InvoiceRequest>
{
    public InvoiceValidator()
    {
        RuleFor(i => i.Month)
            .NotNull().WithMessage("Month can't be null")
            .InclusiveBetween(1, 12).WithMessage("Month must be in range of 1 to 12 inclusive");
        
        RuleFor(i => i.Year)
            .NotNull().WithMessage("Year can't be null")
            .GreaterThan(0).WithMessage("Year must be greater than 0");

        RuleFor(i => i.EmployeeId)
            .NotNull().WithMessage("Employee id can't be null");
    }
}