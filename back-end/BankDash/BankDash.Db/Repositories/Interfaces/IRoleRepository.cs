using BankDash.Model.Enitity;

namespace BankDash.Db.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}
