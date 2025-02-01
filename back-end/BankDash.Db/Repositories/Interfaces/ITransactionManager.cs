namespace BankDash.Db.Repositories.Interfaces
{
    public interface ITransactionManager
    {
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }

}
