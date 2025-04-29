using AutoMapper;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.LoanClaims.Dtos.Tests;

public class LoanClaimProfileTests
{
    private Mapper _mapper;

    public LoanClaimProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<LoanClaimProfile>();
        });
        _mapper = new Mapper(configuration);
    }

    [Fact()]
    public void CreateMap_ForLoanClaimLOanClaimDto_MapsCorrectly()
    {
        // Arrange
        var loanClaim = new LoanClaim()
        {
            Id = 1,
            LoanId = 2,
            IsClaimed = true,
            ClaimedAt = DateTime.UtcNow,
        };

        // Act
        var loanClaimDto = _mapper.Map<LoanClaimDto>(loanClaim);

        // Assert
        loanClaimDto.Should().NotBeNull();
        loanClaimDto.Id.Should().Be(loanClaim.Id);
        loanClaimDto.IsClaimed.Should().Be(loanClaim.IsClaimed);
        loanClaimDto.ClaimedAt.Should().Be(loanClaim.ClaimedAt);
    }
}
