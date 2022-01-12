using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.Interfaces;
using Newtonsoft.Json;
using WebApi.DataBaseProvider;
using WebApi.Entities;
using WebApi.Infrastructure.Extensions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[api]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;

        private readonly DataBaseProviderBase _dbProvider;

        public RoleController(ILogger<PostController> logger, IDataBaseService dataBaseService)
        {
            _logger = logger;
            _dbProvider = dataBaseService.GetProvider();
        }

        [Authorize]
        [HttpGet("/getuserrole")]
        public string GetUserRole()
        {
            // ToDo remove after development testing
            //var user = _dbProvider.GetUserById(Guid.Parse(User.Claims.GetValueByType("Id")));

            //var str = user.ToString();

            var role = _dbProvider.GetRoleNameByUserId(Guid.Parse(User.Claims.GetValueByType("Role")));

            return role.ToString();
        }

        [HttpGet("/getrolenamebyuserid")]
        public string GetRoleNameByUserId(Guid id)
        {
            var role = _dbProvider.GetRoleNameByUserId(id);

            return role?.ToString();
        }
    }
}
