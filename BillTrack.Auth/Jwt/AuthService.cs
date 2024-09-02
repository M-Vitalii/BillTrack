using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BillTrack.Auth.Jwt;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(string username, string password)
    {
        var userId = await CheckCredential(username, password);

        return JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = _configuration["JwtSecretKey"] ?? string.Empty;
                o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()));
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
            });
    }

    private async Task<Guid> CheckCredential(string username, string password)
    {
        var user = await _userRepository.FindAsync(u => u.Email == username && u.Password == HashPassword(password));

        if (user == null)
        {
            throw new NotFoundException($"Entity of type {typeof(User)} with name {username} not found.");
        }

        return user.Id;
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}