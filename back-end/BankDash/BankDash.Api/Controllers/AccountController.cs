namespace BankDash.Api.Controllers
{
    using System.Threading.Tasks;
    using global::BankDash.Model.Dto;
    using global::BankDash.Service;
    using global::BankDash.Service.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    namespace BankDash.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly IAccountService _accountService;
            private readonly ILogger<AccountController> _logger;

            public AccountController(IAccountService accountService, ILogger<AccountController> logger) { 
                _accountService = accountService;
                _logger = logger;
            }

            [HttpGet("user/{userId}")]
            public async Task<IActionResult> GetAccountsForUser(int userId)
            {
                var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
                return Ok(accounts);
            }

            [HttpPost("transfer")]
            public async Task<IActionResult> TransferAmount([FromBody] Transfer transfer)
            {
                try
                {
                    await _accountService.TransferAmountAsync(transfer.FromAccountId, transfer.ToAccountId, transfer.Amount);
                    return Ok("Transfer successful");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Transfer failed from account {transfer.FromAccountId} to account {transfer.ToAccountId}");
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
    }

}
