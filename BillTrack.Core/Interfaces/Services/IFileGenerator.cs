namespace BillTrack.Core.Interfaces.Services;

public interface IFileGenerator
{
    Task<Stream> GenerateFile(Guid entityId);
}