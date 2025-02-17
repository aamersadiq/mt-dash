using BankDash.Model.Dto;
using BankDash.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankDash.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register register)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(register);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registeration failed from user {register.Username}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                var token = await _userService.LoginUserAsync(login);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"Login denied for {login.Username}");
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
