using System.Security.Claims;
using BillTrack.Core.Interfaces.Services;
using FastEndpoints.Security;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BillTrack.Auth.Jwt;

public class FastEndpointsJwtTokenCreator : IJwtTokenCreator
{
    public string CreateToken(Guid userId, string signingKey)
    {
        return JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = signingKey;
                o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()));
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
            });
    }
}