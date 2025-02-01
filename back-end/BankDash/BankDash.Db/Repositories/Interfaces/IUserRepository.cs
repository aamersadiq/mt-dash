using BankDash.Model.Enitity;

namespace BankDash.Db.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }

}
