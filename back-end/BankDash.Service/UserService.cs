using BankDash.Common;
using BankDash.Db.Repositories.Interfaces;
using BankDash.Model.Dto;
using BankDash.Service.Interfaces;

namespace BankDash.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
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
