using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Enitity;
using Microsoft.EntityFrameworkCore;

namespace BankDash.Db.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.User)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
