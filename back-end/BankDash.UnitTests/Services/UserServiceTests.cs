using BankDash.Common;
using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Dto;
using BankDash.Model.Enitity;
using BankDash.Service;
using BankDash.Service.Interfaces;
using Moq;

namespace BankDash.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Fact]
        public async Task LoginUserAsync_ValidUser_ReturnToken()
        {
            // Arrange
            var login = new Login { Username = "testuser", Password = "password" };
            var user = new User { Id = 1, Username = "testuser", PasswordHash = PasswordHelper.HashPassword("password") };
            var token = "dummy-jwt-token";

            _userRepositoryMock.Setup(u => u.GetUserByUsernameAsync(login.Username)).ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.GenerateJwtToken(user)).Returns(token);

            // Act
            var result = await _userService.LoginUserAsync(login);

            // Assert
            Assert.Equal(token, result);
            _userRepositoryMock.Verify(u => u.GetUserByUsernameAsync(login.Username), Times.Once);
            _tokenServiceMock.Verify(t => t.GenerateJwtToken(user), Times.Once);
        }

        [Fact]
        public async Task LoginUserAsync_InvalidUser_ThrowUnauthorizedAccessException()
        {
            // Arrange
            var login = new Login { Username = "invaliduser", Password = "password" };

            _userRepositoryMock.Setup(u => u.GetUserByUsernameAsync(login.Username)).ReturnsAsync((User) null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginUserAsync(login));
            _userRepositoryMock.Verify(u => u.GetUserByUsernameAsync(login.Username), Times.Once);
        }

        [Fact]
        public async Task LoginUserAsync_InvalidPassword_ThrowUnauthorizedAccessException()
        {
            // Arrange
            var login = new Login { Username = "testuser", Password = "wrongpassword" };
            var user = new User { Id = 1, Username = "testuser", PasswordHash = PasswordHelper.HashPassword("password") };

            _userRepositoryMock.Setup(u => u.GetUserByUsernameAsync(login.Username)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginUserAsync(login));
            _userRepositoryMock.Verify(u => u.GetUserByUsernameAsync(login.Username), Times.Once);
        }
    }
}