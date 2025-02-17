﻿using BankDash.Model.Dto;
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
            var user = await _userService.RegisterUserAsync(register);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var token = await _userService.LoginUserAsync(login);
            return Ok(new { Token = token });
        }
    }
}
