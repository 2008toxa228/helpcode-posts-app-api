using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Infrastructure.Jwt;
using System.Text;
using WebApi.DataBaseProvider;
using WebApi.DataBaseProvider.Interfaces;
using WebApi.Entities;
using Newtonsoft.Json;
using WebApi.Services.Interfaces;
using WebApi.Models;
using System.Threading.Tasks;
using System.Net;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var response = await _userService.Register(model);

            // ToDo add more status codes to register.
            if (response != HttpStatusCode.Created)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }
    }
}