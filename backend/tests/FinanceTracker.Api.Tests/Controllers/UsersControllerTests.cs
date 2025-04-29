using FinanceTracker.Application.Users.Dtos;
using FinanceTracker.Application.Users.Queries.GetUserInfoByEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FinanceTracker.Api.Controllers.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UsersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetUserInfoByEmail_ForValidEmail_Returns200Ok()
        {
            // Arrange
            var email = "test@example.com";
            var expectedResult = new UserInfoDto { Id = "1", Email = email };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetUserInfoByEmailQuery>(q => q.Email == email), default))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetUserInfoByEmail(email);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(200, okResult.StatusCode);
            Xunit.Assert.Equal(expectedResult, okResult.Value);

            _mediatorMock.Verify(
                m => m.Send(It.Is<GetUserInfoByEmailQuery>(q => q.Email == email), default),
                Times.Once
            );
        }
    }
}
