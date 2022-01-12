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
    [Route("[user]")]
    public class UserControlelr : ControllerBase
    {
        private readonly ILogger<PostController> _logger;

        private readonly DataBaseProviderBase _dbProvider;

        public UserControlelr(ILogger<PostController> logger, IDataBaseService dataBaseService)
        {
            _logger = logger;
            _dbProvider = dataBaseService.GetProvider();
        }

        [HttpGet("/getusers")]
        public IEnumerable<User> GetUsers(int page, int perPage = 100)
        {
            var users = _dbProvider.GetUsers(page, perPage);

            return users;
        }

        [HttpGet("/getuserbyid")]
        public User GetUserById(Guid id)
        {
            return _dbProvider.GetUserById(id);
        }

        [Authorize]
        [HttpGet("/getuser")]
        public User GetUser()
        {
            return _dbProvider.GetUserById(Guid.Parse(User.Claims.GetValueByType("Id")));
        }
    }
}
