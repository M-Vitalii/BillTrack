using System.Security.Claims;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Interfaces.Utils;
using BillTrack.Domain.Entities;
using FastEndpoints.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BillTrack.Auth.Jwt;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenCreator _jwtTokenCreator;

    public AuthService(
        IGenericRepository<User> userRepository, 
        IConfiguration configuration,
        IPasswordHasher passwordHasher, 
        IJwtTokenCreator jwtTokenCreator)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _jwtTokenCreator = jwtTokenCreator;
    }

    public async Task<string> GenerateToken(string username, string password)
    {
        var userId = await ValidateAndGetUserId(username, password);

        return CreateJwtToken(userId);
    }

    private async Task<Guid> ValidateAndGetUserId(string username, string password)
    {
        var user = await _userRepository.FindAsync(u => u.Email == username);

        if (user == null || !_passwordHasher.Verify(user.Password, password))
        {
            throw new UnauthorizedAccessException("The user name and/or password is invalid.");
        }

        return user.Id;
    }

    private string CreateJwtToken(Guid userId)
    {
        var signingKey = _configuration["JwtSecretKey"] ??
                         throw new InvalidOperationException("JWT Secret Key is missing.");

        return _jwtTokenCreator.CreateToken(userId, signingKey);
    }
}