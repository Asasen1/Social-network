using Application.DataAccess;
using Application.Features;
using Application.Features.Login;
using Application.Providers;
using Domain.AgregateRoot;
using Domain.Common;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Features;

public class LoginHandlerTests
{
    private readonly Mock<ITransaction> _transaction = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IJwtProvider> _jwtProvider = new();
    public LoginHandlerTests()
    {
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsLoginResponse()
    {
        // arrange
        var ct = new CancellationToken();
        
        var userMock = User.Create(
            Email.Create("test@gmail.com").Value,
            BCrypt.Net.BCrypt.HashPassword("password"),
            FullName.Create("test", "test").Value,
            "",
            null,
            null);
        
        _userRepository.Setup(u => u.GetByEmail("test@gmail.com", ct))
            .ReturnsAsync(userMock.Value);
        _transaction.Setup(t => t.SaveChangesAsync(ct))
            .ReturnsAsync(1);
        _jwtProvider.Setup(j => j.GenerateAccessToken(userMock.Value))
            .Returns("token");
        _jwtProvider.Setup(j => j.GenerateRefreshToken())
            .Returns(new RefreshToken("token", DateTime.Now));
        
        var request = new LoginRequest("test@gmail.com", "password");
        var sut = new LoginHandler(_transaction.Object, _userRepository.Object, _jwtProvider.Object);

        // act
        var result = await sut.Handle(request, ct);
        
        // assert
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeNull();
        result.IsFailure.Should().Be(false);
        result.Error.Should().Be(Error.None);
        result.Value.AccessToken.Should().BeOfType<string>();
        result.Value.RefershToken.Should().BeOfType<string>();
        result.Value.Role.Should().NotBeEmpty();
        
    }
}