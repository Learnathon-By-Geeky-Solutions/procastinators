using AutoMapper;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.Users.Dtos.Tests;

public class UserInfoProfileTests
{
    private Mapper _mapper;

    public UserInfoProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserInfoProfile>();
        });
        _mapper = new Mapper(configuration);
    }

    [Fact()]
    public void CreateMap_ForUserToUserInfoDto_MapsCorrectly()
    {
        // Arrange
        var user = new User()
        {
            RegisteredAt = DateTime.UtcNow,
            Id = "test",
            Email = "gesg@.ds",
            UserName = "buhis",
        };

        // Act
        var userInfoDto = _mapper.Map<UserInfoDto>(user);

        // Assert
        userInfoDto.Should().NotBeNull();
        userInfoDto.Id.Should().Be(user.Id);
        userInfoDto.Email.Should().Be(user.Email);
        userInfoDto.UserName.Should().Be(user.UserName);
    }
}
