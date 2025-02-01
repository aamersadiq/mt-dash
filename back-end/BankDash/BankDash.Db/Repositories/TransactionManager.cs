using BankDash.Db.Repositories.Interfaces;

namespace BankDash.Db.Repositories
{
    public class TransactionManager : ITransactionManager
    {
        private readonly AppDbContext _context;

        public TransactionManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await operation();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

}
