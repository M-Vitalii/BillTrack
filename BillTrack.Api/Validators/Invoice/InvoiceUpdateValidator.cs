using BillTrack.Core.Contracts.Invoice;
using FastEndpoints;

namespace BillTrack.Api.Validators.Invoice;

public class InvoiceUpdateValidator : Validator<InvoiceUpdateRequest>
{
    public InvoiceUpdateValidator()
    {
        RuleFor(i => i.Id)
            .NotEmpty().WithMessage("Id can't be empty");
        
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