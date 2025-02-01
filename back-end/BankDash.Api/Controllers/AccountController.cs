using BankDash.Model.Dto;
using BankDash.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankDash.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [Authorize(Roles = "AccountRead, AccountManage")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAccountsForUser(int userId)
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
            return Ok(accounts);
        }

        [Authorize(Roles = "AccountManage")]
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


