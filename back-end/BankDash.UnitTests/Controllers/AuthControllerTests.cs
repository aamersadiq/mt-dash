using BankDash.Api.Controllers;
using BankDash.Model.Dto;
using BankDash.Model.Enitity;
using BankDash.Service.Interfaces;
using BankDash.UnitTests.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankDash.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_userServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Login_ValidLogin_ReturnOk()
        {
            // Arrange
            var login = new Login { Username = "test-user", Password = "password" };
            var token = "dummy-jwt-token";

            _userServiceMock.Setup(service => service.LoginUserAsync(login)).ReturnsAsync(token);

            // Act
            var result = await _authController.Login(login) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(token, JsonValueHelper.GetValue(result?.Value, "Token"));
            _userServiceMock.Verify(service => service.LoginUserAsync(login), Times.Once);
        }

        [Fact]
        public async Task Register_ValidUser_ShouldReturnOk()
        {
            // Arrange
            var register = new Register { Username = "testuser", Password = "password" };
            var user = new User { Username = "testuser" };
            _userServiceMock.Setup(u => u.RegisterUserAsync(register)).ReturnsAsync(user);

            // Act
            var result = await _authController.Register(register);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, okResult.Value);
        }
    }
}
