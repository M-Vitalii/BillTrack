namespace BillTrack.Core.Interfaces.Services;

public interface IPdfGenerator
{
    Task<Stream> GeneratePdfStream(Guid entityId);
}