using AutoMapper;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.InstallmentClaims.Dtos.Tests;

public class InstallmentClaimProfileTests
{
    private Mapper _mapper;

    public InstallmentClaimProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InstallmentClaimProfile>();
        });
        _mapper = new Mapper(configuration);
    }

    [Fact()]
    public void CreateMap_ForInstallClaimToInstallClaimDto_MapsCorrectly()
    {
        // Arrange
        var installmentClaims = new InstallmentClaim()
        {
            Id = 1,
            InstallmentId = 2,
            IsClaimed = true,
            ClaimedAt = DateTime.UtcNow,
        };

        // Act
        var installmentClaimDto = _mapper.Map<InstallmentClaimDto>(installmentClaims);

        // Assert
        installmentClaimDto.Should().NotBeNull();
        installmentClaimDto.Id.Should().Be(installmentClaims.Id);
        installmentClaimDto.IsClaimed.Should().Be(installmentClaims.IsClaimed);
        installmentClaimDto.ClaimedAt.Should().Be(installmentClaims.ClaimedAt);
    }
}
