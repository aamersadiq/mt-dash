using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Enitity;
using BankDash.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace BankDash.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, ITransactionManager transactionManager, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return await _accountRepository.GetAccountsByUserIdAsync(userId);
        }

        public async Task TransferAmountAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero");
            }

            var fromAccount = await _accountRepository.GetAccountByIdAsync(fromAccountId);
            var toAccount = await _accountRepository.GetAccountByIdAsync(toAccountId);

            if (fromAccount == null)
            {
                throw new Exception($"Account not found: {fromAccountId}");
            }

            if (toAccount == null)
            {
                throw new Exception($"Account not found: {toAccountId}");
            }

            if (fromAccount.Balance < amount)
            {
                throw new Exception("Insufficient balance");
            }

            var transactionDate = DateTime.UtcNow;
            fromAccount.Balance -= amount;
            var debitTransaction = new Transaction
            {
                AccountId = fromAccountId,
                Type = TransactionType.Debit.ToString(),
                Amount = amount,
                TransactionDate = transactionDate
            };

            toAccount.Balance += amount;
            var creditTransaction = new Transaction
            {
                AccountId = toAccountId,
                Type = TransactionType.Credit.ToString(),
                Amount = amount,
                TransactionDate = transactionDate
            };

            try
            {
                await _transactionManager.ExecuteInTransactionAsync(async () =>
                {
                    await _transactionRepository.AddTransactionAsync(debitTransaction);
                    await _transactionRepository.AddTransactionAsync(creditTransaction);

                    await _accountRepository.SaveChangesAsync();
                    await _transactionRepository.SaveChangesAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transfer failed");
                throw;
            }
        }

    }
}
