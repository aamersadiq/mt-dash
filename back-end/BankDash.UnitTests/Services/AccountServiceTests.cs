using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Enitity;
using BankDash.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankDash.UnitTests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ITransactionManager> _transactionManagerMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _transactionManagerMock = new Mock<ITransactionManager>();
            _loggerMock = new Mock<ILogger<AccountService>>();

            _accountService = new AccountService(
                _accountRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _transactionManagerMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAccountsByUserIdAsync_ValidUserId_ReturnAccounts()
        {
            // Arrange
            var userId = 1;
            var accounts = new List<Account> {
                new Account { Id = 1, UserId = userId, Balance = 1000 },
                new Account { Id = 2, UserId = userId, Balance = 2000 }
            };

            _accountRepositoryMock.Setup(repo => repo.GetAccountsByUserIdAsync(userId)).ReturnsAsync(accounts);

            // Act
            var result = await _accountService.GetAccountsByUserIdAsync(userId);

            // Assert
            Assert.Equal(accounts, result);
            _accountRepositoryMock.Verify(repo => repo.GetAccountsByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task TransferAmountAsync_WhenValid_TransferFunds()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 100;
            var fromAccount = new Account { Id = fromAccountId, Balance = 500 };
            var toAccount = new Account { Id = toAccountId, Balance = 1000 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(toAccountId)).ReturnsAsync(toAccount);
            _transactionManagerMock.Setup(tm => tm.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(async action => await action());

            // Act
            await _accountService.TransferAmountAsync(fromAccountId, toAccountId, amount);

            // Assert
            Assert.Equal(400, fromAccount.Balance);
            Assert.Equal(1100, toAccount.Balance);
            _accountRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.AddTransactionAsync(It.IsAny<Transaction>()), Times.Exactly(2));
            _transactionRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task TransferAmountAsync_WhenInsufficientBalance_ThrowException()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 1000;
            var fromAccount = new Account { Id = fromAccountId, Balance = 500 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(toAccountId)).ReturnsAsync(new Account());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _accountService.TransferAmountAsync(fromAccountId, toAccountId, amount));
            Assert.Equal("Insufficient balance", exception.Message);
        }

        [Fact]
        public async Task TransferAmountAsync_WhenAccountNotFound_ThrowException()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 100;

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(fromAccountId)).ReturnsAsync((Account) null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _accountService.TransferAmountAsync(fromAccountId, toAccountId, amount));
            Assert.Equal($"Account not found: {fromAccountId}", exception.Message);
        }

        [Fact]
        public async Task TransferAmountAsync_WhenAmountIsZeroOrLess_ThrowArgumentException()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _accountService.TransferAmountAsync(fromAccountId, toAccountId, 0));
            await Assert.ThrowsAsync<ArgumentException>(() => _accountService.TransferAmountAsync(fromAccountId, toAccountId, -100));
        }
    }
}
