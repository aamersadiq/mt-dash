using BankDash.Model.Enitity;

namespace BankDash.Db.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}