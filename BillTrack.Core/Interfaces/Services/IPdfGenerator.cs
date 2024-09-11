namespace BillTrack.Core.Interfaces.Services;

public interface IPdfGenerator
{
    Task<Stream> GeneratePdf(Guid invoiceId);
}