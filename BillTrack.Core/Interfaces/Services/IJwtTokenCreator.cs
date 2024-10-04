namespace BillTrack.Core.Interfaces.Services;

public interface IJwtTokenCreator
{
    string CreateToken(Guid userId, string signingKey);
}