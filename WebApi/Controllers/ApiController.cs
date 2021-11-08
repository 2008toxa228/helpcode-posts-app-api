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
using WebApi.DataBaseProvider.Interfaces;
using WebApi.Entities;
using WebApi.Infrastructure.Extensions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        private readonly IDataBaseProvider _dbProvider;

        public ApiController(ILogger<ApiController> logger, IDataBaseService dataBaseService)
        {
            _logger = logger;
            _dbProvider = dataBaseService.GetProvider();
        }

        [Authorize]
        [HttpGet]
        public string GetUserRole()
        {
            // ToDo remove after development testing
            //var user = _dbProvider.GetUserById(Guid.Parse(User.Claims.GetValueByType("Id")));

            //var str = user.ToString();

            var role = _dbProvider.GetRoleById(Guid.Parse(User.Claims.GetValueByType("Role")));

            return role.ToString();
        }
    }
}
