namespace BillTrack.Core.Interfaces.Services;

public interface IAuthService
{
    Task<string> GenerateToken(string username, string password);
}