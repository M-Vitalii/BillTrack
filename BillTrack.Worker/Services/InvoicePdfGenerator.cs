using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models.Worker;
using BillTrack.Domain.Entities;
using BillTrack.Worker.Models;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BillTrack.Worker.Services;
public class InvoicePdfGenerator : IPdfGenerator
{
    private readonly IGenericRepository<Invoice> _invoiceRepository;
    private readonly IEmployeeSalaryCalculator _employeeSalaryCalculator;

    public InvoicePdfGenerator(IGenericRepository<Invoice> invoiceRepository, IEmployeeSalaryCalculator employeeSalaryCalculator)
    {
        _invoiceRepository = invoiceRepository;
        _employeeSalaryCalculator = employeeSalaryCalculator;
    }

    public async Task<Stream> GeneratePdfStream(Guid invoiceId)
    {
        Settings.License = LicenseType.Community;

        var invoice = await _invoiceRepository.GetByIdAsync(
            invoiceId, 
            i => i.Employee, 
            i => i.Employee.Department, 
            i => i.Employee.Project);

        var employeeWorkSummary = await _employeeSalaryCalculator.CalculateEmployeeSalaryAsync(invoice);
        var stream = new MemoryStream();
        var document = new InvoiceDocument(ConvertToInvoiceModel(invoice, employeeWorkSummary));
        
        document.GeneratePdf(stream);
        stream.Position = 0;

        return stream;
    }
    
    private InvoiceModel ConvertToInvoiceModel(Invoice invoice, EmployeeWorkSummary employeeWorkSummary)
    {
        return new InvoiceModel
        {
            InvoiceId = invoice.Id,
            IssueDate = DateTime.Now,
            EmployeeWorkSummary = employeeWorkSummary
        };
    }
}