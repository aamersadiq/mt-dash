﻿using BankDash.Api.Controllers;
using BankDash.Model.Dto;
using BankDash.Service.Interfaces;
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
            Assert.Equal(token, result.Value.GetType().GetProperty("Token").GetValue(result.Value, null));
            _userServiceMock.Verify(service => service.LoginUserAsync(login), Times.Once);
        }

        [Fact]
        public async Task Login_UnauthorizedAccessExceptionThrown_ReturnUnauthorized()
        {
            // Arrange
            var login = new Login { Username = "test-user", Password = "password" };
            var errorMessage = "Invalid username or password.";

            _userServiceMock.Setup(service => service.LoginUserAsync(login)).ThrowsAsync(new UnauthorizedAccessException(errorMessage));

            // Act
            var result = await _authController.Login(login) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal(errorMessage, result.Value);
        }
    }
}
