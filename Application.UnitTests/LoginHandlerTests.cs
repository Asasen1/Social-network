using Application.DataAccess;
using Application.Features;
using Application.Features.Login;
using Application.Providers;
using Domain.AgregateRoot;
using Domain.Common;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests;

public class LoginHandlerTests
{
    private readonly Mock<ITransaction> transaction = new();
    private readonly Mock<IUserRepository> userRepository = new();
    private readonly Mock<IJwtProvider> jwtProvider = new();
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
        userRepository
            .Setup(u => u.GetByEmail("test@gmail.com", ct)).ReturnsAsync(userMock.Value);
        transaction.Setup(t => t.SaveChangesAsync(ct)).ReturnsAsync(1);
        jwtProvider.Setup(j => j.GenerateAccessToken(userMock.Value)).Returns("token");
        jwtProvider.Setup(j => j.GenerateRefreshToken()).Returns(new RefreshToken("token", DateTime.Now));
        var handler = new LoginHandler(transaction.Object, userRepository.Object, jwtProvider.Object);
        var request = new LoginRequest("test@gmail.com", "password");

        // act
        var sut = await handler.Handle(request, ct);
        
        // assert
        sut.IsSuccess.Should().Be(true);
        sut.Value.Should().NotBeNull();
        sut.IsFailure.Should().Be(false);
        sut.Error.Should().Be(Error.None);
        sut.Value.AccessToken.Should().BeOfType<string>();
        sut.Value.RefershToken.Should().BeOfType<string>();
        sut.Value.Role.Should().NotBeEmpty();
        
    }
}