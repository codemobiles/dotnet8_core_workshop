using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet8_hero.DTOs.Account;
using dotnet8_hero.Entities;
using dotnet8_hero.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountService AccountService { get; set; }
        public AccountController(IAccountService accountService)
        {
            this.AccountService = accountService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            try
            {
                var account = request.Adapt<Account>();
                await AccountService.Register(account);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            var account = await AccountService.Login(loginRequest.Username, loginRequest.Password);
            if (account == null)
            {
                return Unauthorized();
            }

            return Ok(new { token = AccountService.GenerateToken(account) });

        }

    }
}