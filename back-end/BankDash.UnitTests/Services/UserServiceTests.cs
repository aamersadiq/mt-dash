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
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _accountRepositoryMock.Object,
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

        [Fact]
        public async Task RegisterUserAsync_ValidUser_RegisterUserAndCreateAccount()
        {
            // Arrange
            var userToRegister = new Register { Username = "test-user", Password = "password" };
            var roleForRegister = new Role { Id = 1, Name = RoleType.AccountRead.ToString() };

            _roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(RoleType.AccountRead.ToString())).ReturnsAsync(roleForRegister);

            // Act
            var user = await _userService.RegisterUserAsync(userToRegister);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(userToRegister.Username, user.Username);
            _roleRepositoryMock.Verify(r => r.GetRoleByNameAsync(RoleType.AccountRead.ToString()), Times.Once);
            _userRepositoryMock.Verify(u => u.AddUserAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _accountRepositoryMock.Verify(a => a.AddAccountAsync(It.IsAny<Account>()), Times.Once);
            _accountRepositoryMock.Verify(a => a.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_InvalidRole_ThroExceptioWheRoleNotFound()
        {
            // Arrange
            var userToRegister = new Register { Username = "testuser", Password = "password" };

            _roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(RoleType.AccountRead.ToString())).ReturnsAsync((Role) null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.RegisterUserAsync(userToRegister));
            _roleRepositoryMock.Verify(r => r.GetRoleByNameAsync(RoleType.AccountRead.ToString()), Times.Once);
            _userRepositoryMock.Verify(u => u.AddUserAsync(It.IsAny<User>()), Times.Never);
            _accountRepositoryMock.Verify(a => a.AddAccountAsync(It.IsAny<Account>()), Times.Never);
        }
    }
}