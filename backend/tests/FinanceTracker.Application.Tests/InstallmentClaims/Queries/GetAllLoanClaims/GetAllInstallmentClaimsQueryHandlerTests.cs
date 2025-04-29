using AutoMapper;
using FinanceTracker.Application.InstallmentClaims.Dtos;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Installments.Queries.GetAllInstallments;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.InstallmentClaims.Queries.GetAllLoanClaims.Tests;

public class GetAllInstallmentClaimsQueryHandlerTests
{
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllInstallmentClaimsQueryHandler _handler;
    private readonly string _userId = "test";
    private readonly UserDto _user = new("test", "test@test.com");

    public GetAllInstallmentClaimsQueryHandlerTests()
    {
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new GetAllInstallmentClaimsQueryHandler(
            _userContextMock.Object,
            _installmentRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllInstallmentsForCurrentUser()
    {
        // Arrange
        var query = new GetAllInstallmentClaimsQuery { };

        var installmentClaim = new List<InstallmentClaim>
        {
            new InstallmentClaim
            {
                Id = 1,
                InstallmentId = 2,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
            new InstallmentClaim
            {
                Id = 3,
                InstallmentId = 4,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
        };

        var installmentClaimDtos = new List<InstallmentClaimDto>
        {
            new InstallmentClaimDto
            {
                Id = 1,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
            new InstallmentClaimDto
            {
                Id = 2,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
        };

        _installmentRepositoryMock
            .Setup(repo => repo.GetAllInstallmentClaimsAsync(_userId))
            .ReturnsAsync(installmentClaim);

        _mapperMock
            .Setup(m =>
                m.Map<IEnumerable<InstallmentClaimDto>>(It.IsAny<IEnumerable<InstallmentClaim>>())
            )
            .Returns(installmentClaimDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(installmentClaimDtos);
        _installmentRepositoryMock.Verify(
            repo => repo.GetAllInstallmentClaimsAsync(_userId),
            Times.Once
        );
        _mapperMock.Verify(
            m => m.Map<IEnumerable<InstallmentClaimDto>>(installmentClaim),
            Times.Once
        );
    }
}
