using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController()
        {
        }

        [HttpGet("")]
        public ActionResult<string> GetSomething()
        {
            return "Yes, it works!";
        }
        
        
    }
}