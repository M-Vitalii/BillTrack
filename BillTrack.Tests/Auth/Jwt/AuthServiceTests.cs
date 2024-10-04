using System.Linq.Expressions;
using BillTrack.Auth.Jwt;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Interfaces.Utils;
using BillTrack.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BillTrack.Tests.Auth.Jwt;

public class AuthServiceTests
{
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenCreator> _mockJwtTokenCreator;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenCreator = new Mock<IJwtTokenCreator>();

        _authService = new AuthService(
            _mockUserRepository.Object,
            _mockConfiguration.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenCreator.Object
        );
    }

    [Fact]
    public async Task GenerateToken_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var username = "test@example.com";
        var password = "password123";
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = username, Password = "hashedPassword" };

        _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(It.IsAny<string>(), password))
            .Returns(true);
        _mockConfiguration.Setup(c => c["JwtSecretKey"])
            .Returns("ThisIsAVeryLongSecretKeyForTesting");
        _mockJwtTokenCreator.Setup(j => j.CreateToken(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns("mocked_jwt_token");

        // Act
        var token = await _authService.GenerateToken(username, password);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task GenerateToken_InvalidUsername_ThrowsNotFoundException()
    {
        // Arrange
        var username = "nonexistent@example.com";
        var password = "password123";

        _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);
        _mockJwtTokenCreator.Setup(j => j.CreateToken(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns("mocked_jwt_token");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _authService.GenerateToken(username, password));
    }

    [Fact]
    public async Task GenerateToken_InvalidPassword_ThrowsNotFoundException()
    {
        // Arrange
        var username = "test@example.com";
        var password = "wrongPassword";
        var user = new User { Id = Guid.NewGuid(), Email = username, Password = "hashedPassword" };

        _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(It.IsAny<string>(), password))
            .Returns(false);
        _mockJwtTokenCreator.Setup(j => j.CreateToken(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns("mocked_jwt_token");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _authService.GenerateToken(username, password));
    }

    [Fact]
    public async Task GenerateToken_MissingJwtSecretKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var username = "test@example.com";
        var password = "password123";
        var user = new User { Id = Guid.NewGuid(), Email = username, Password = "hashedPassword" };

        _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(It.IsAny<string>(), password))
            .Returns(true);
        _mockConfiguration.Setup(c => c["JwtSecretKey"])
            .Returns((string)null);
        _mockJwtTokenCreator.Setup(j => j.CreateToken(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns("mocked_jwt_token");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _authService.GenerateToken(username, password));
    }
}