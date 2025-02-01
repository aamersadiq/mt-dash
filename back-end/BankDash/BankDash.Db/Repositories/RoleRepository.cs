using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Enitity;
using Microsoft.EntityFrameworkCore;

namespace BankDash.Db.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }

}
