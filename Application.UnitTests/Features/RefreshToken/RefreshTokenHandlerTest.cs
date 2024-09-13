using System.Security.Claims;
using Application.DTO;
using Application.Features.RefreshToken;
using Application.Providers;
using Domain.AgregateRoot;
using Domain.Constants;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Features.RefreshToken;

public class RefreshTokenHandlerTest
{
    private readonly Mock<IJwtProvider> _jwtProviderMock = new();
    private Mock<ClaimsPrincipal> _claimPrincipalMock = new();
    
    [Fact]
    public async Task Handle_RefreshToken_ReturnValidTokenDto()
    {
        //arrange
        var claims = new List<Claim>
        {
            new Claim(AuthenticationConstants.Permission, "test"),
            new Claim(AuthenticationConstants.UserId, Guid.NewGuid().ToString()),
            new Claim(AuthenticationConstants.Role, "USER"),
        };

        _claimPrincipalMock.Setup(c => c.Claims).Returns(claims);
        var ct = new CancellationToken();
        var tokenDto = new TokenDto("token", "token");
        var userMock = User.Create(
            Email.Create("test@gmail.com").Value,
            BCrypt.Net.BCrypt.HashPassword("password"),
            FullName.Create("test", "test").Value,
            "",
            null,
            null);
        _jwtProviderMock.Setup(j => j.GetPrincipalFromExpiredToken("token"))
            .Returns(_claimPrincipalMock.Object);
        _jwtProviderMock.Setup(j => j.CheckExpired(_claimPrincipalMock.Object, "token", ct))
            .ReturnsAsync(userMock);
        _jwtProviderMock.Setup(j => j.UpdateTokens(userMock.Value, ct))
            .ReturnsAsync(tokenDto);
        var handler = new RefreshTokenHandler(_jwtProviderMock.Object);
        //act
        var sut = await handler.Handle(tokenDto, ct);

        //assert
        sut.IsSuccess.Should().Be(true);
        sut.Value.Should().BeOfType<TokenDto>();
        sut.Value.Should().NotBeNull();
    }
}