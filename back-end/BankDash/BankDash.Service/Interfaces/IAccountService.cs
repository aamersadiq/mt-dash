using BankDash.Model.Enitity;

namespace BankDash.Service.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId);
        Task TransferAmountAsync(int fromAccountId, int toAccountId, decimal amount);
    }
}
