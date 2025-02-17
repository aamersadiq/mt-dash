
using BankDash.Api.Controllers;
using BankDash.Model.Dto;
using BankDash.Model.Enitity;
using BankDash.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankDash.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _accountController = new AccountController(_accountServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAccountsForUser_WithAccounts_ReturnOk()
        {
            // Arrange
            var userId = 1;
            var accounts = new List<Account>{
                new Account { Id = 1, UserId = userId, Balance = 1000 },
                new Account { Id = 2, UserId = userId, Balance = 2000 }
            };

            _accountServiceMock.Setup(service => service.GetAccountsByUserIdAsync(userId)).ReturnsAsync(accounts);

            // Act
            var result = await _accountController.GetAccountsForUser(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(accounts, result.Value);
            _accountServiceMock.Verify(service => service.GetAccountsByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task TransferAmount_ValidTransfer_ShouldReturnOk()
        {
            // Arrange
            var transfer = new Transfer { FromAccountId = 1, ToAccountId = 2, Amount = 100 };;

            // Act
            var result = await _accountController.TransferAmount(transfer) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Transfer successful", result.Value.GetType().GetProperty("message").GetValue(result.Value, null));
            _accountServiceMock.Verify(service => service.TransferAmountAsync(transfer.FromAccountId, transfer.ToAccountId, transfer.Amount), Times.Once);
        }

        [Fact]
        public async Task TransferAmount_InvalidTransfer_ReturnBadRequest()
        {
            // Arrange
            var transfer = new Transfer { FromAccountId = 1, ToAccountId = 2, Amount = 100 };
            var errorMessage = "Insufficent funds.";

            _accountServiceMock.Setup(service => service.TransferAmountAsync(transfer.FromAccountId, transfer.ToAccountId, transfer.Amount))
                               .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _accountController.TransferAmount(transfer) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(errorMessage, result.Value.GetType().GetProperty("message").GetValue(result.Value, null));
        }
    }
}
