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
using WebApi.Entities;
using Newtonsoft.Json;
using WebApi.Services.Interfaces;
using WebApi.Models;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using WebApi.Infrastructure.Extensions;

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

        [HttpPost("/signin")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }

        [HttpPost("/refresh")]
        public IActionResult Refresh([FromBody]string refreshToken)
        {
            var refreshRequest = new RefreshAccessTokenRequest()
            {
                UserId = Guid.Parse(JwtManager.GetClaimsFromToken(refreshToken).GetValueByType("Id")),
                RefreshToken = refreshToken
            };

            var response = _userService.RefreshAccessToken(refreshRequest);

            if (response == null)
            {
                return BadRequest(new { message = "Refreshing access token attempt is failed" });
            }

            return Ok(response);
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest model)
        {
            var response = await _userService.Register(model);

            // ToDo add more status codes to register.
            if (response != HttpStatusCode.Created)
            {
                return BadRequest(new { message = "Register attempt is failed" });
            }

            return Ok(response);
        }
    }
}