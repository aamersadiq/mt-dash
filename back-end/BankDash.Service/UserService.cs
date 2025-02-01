using BankDash.Common;
using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Dto;
using BankDash.Model.Enitity;
using BankDash.Service.Interfaces;

namespace BankDash.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAccountRepository accountRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        public async Task<User> RegisterUserAsync(Register register)
        {
            var role = await _roleRepository.GetRoleByNameAsync(RoleType.AccountRead.ToString());
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            var user = new User
            {
                Username = register.Username,
                PasswordHash = PasswordHelper.HashPassword(register.Password),
                UserRoles = new List<UserRole> { new UserRole { RoleId = role.Id } }
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            var account = new Account
            {
                UserId = user.Id,
                Balance = 0
            };

            await _accountRepository.AddAccountAsync(account);
            await _accountRepository.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginUserAsync(Login login)
        {
            var user = await _userRepository.GetUserByUsernameAsync(login.Username);

            if (user == null || !PasswordHelper.VerifyPassword(login.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _tokenService.GenerateJwtToken(user);
        }

    }
}
