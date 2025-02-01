using BankDash.Model.Enitity;

namespace BankDash.Db.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(int id);
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId);
        Task AddAccountAsync(Account account);
        Task SaveChangesAsync();
    }

}
