using System.Text.Json;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using BillTrack.Persistence;
using BillTrack.Worker.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BillTrack.Worker.Services;
public class InvoicePdfGenerator : IFileGenerator
{
    private readonly IGenericRepository<Invoice> _invoiceRepository;

    public InvoicePdfGenerator(IGenericRepository<Invoice> invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<Stream> GenerateFile(Guid invoiceId)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var invoice = await _invoiceRepository.GetByIdAsync(
            invoiceId, 
            i => i.Employee, 
            i => i.Employee.Department, 
            i => i.Employee.Project);
        
        var stream = new MemoryStream();
        
        var document = new InvoiceDocument(MapToInvoiceModel(invoice));
        document.GeneratePdf(stream);
        stream.Position = 0;

        return stream;
    }
    
    private InvoiceModel MapToInvoiceModel(Invoice invoice)
    {
        return new InvoiceModel
        {
            InvoiceId = invoice.Id,
            IssueDate = DateTime.Now,
            Employee = invoice.Employee
        };
    }
}