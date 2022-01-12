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
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
    {
        [ApiController]
        [Route("[category]")]
        public class QueriesController : ControllerBase
        {
            private readonly ILogger<PostController> _logger;

            private readonly DataBaseProviderBase _dbProvider;

            public QueriesController(ILogger<PostController> logger, IDataBaseService dataBaseService)
            {
                _logger = logger;
                _dbProvider = dataBaseService.GetProvider();
            }

        [HttpGet("/query1")]
        public IEnumerable<object> Query1(string username, DateTime startDate, DateTime endDate)
        {
            return _dbProvider.Query1(username, startDate, endDate);
        }
        [HttpGet("/query2")]
        public IEnumerable<object> Query2(string username, DateTime startDate, DateTime endDate)
        {
            return _dbProvider.Query2(username, startDate, endDate);
        }
        [HttpGet("/query3")]
        public IEnumerable<object> Query3(string postTitle)
        {
            return _dbProvider.Query3(postTitle);
        }
    }
    }