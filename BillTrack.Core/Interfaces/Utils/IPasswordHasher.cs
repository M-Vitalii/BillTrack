namespace BillTrack.Core.Interfaces.Utils;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string passwordHash, string inputPassword);
}