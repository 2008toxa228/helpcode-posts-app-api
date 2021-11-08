using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Infrastructure.Hash;
using WebApi.Infrastructure.Jwt;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        private readonly IDataBaseService _dataBaseService;

        private readonly Guid _defaultRole;

        public UserService(IConfiguration configuration, IDataBaseService dataBaseService)
        {
            _defaultRole = Guid.Parse("858dcdcd-7093-4294-aa85-db86902a3d90");
            _dataBaseService = dataBaseService;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            var user = _dataBaseService.GetProvider().GetUserByEmail(request.Email);

            //if (user == null)
            //{
            //    return null;
            //}

            if (user != null && PasswordManager.Verify(request.Password, user.PasswordHash))
            {
                var response = new AuthenticateResponse(user, JwtManager.GetAccessToken(user), JwtManager.GetRefreshToken(user));

                return response;
            }

            return null;
        }

        public async Task<HttpStatusCode> Register(RegisterRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                RoleId = _defaultRole,
                Email = request.Email,
                Username = request.UserName,
                PasswordHash = PasswordManager.GetHash(request.Password),
                Reputation = 0,
            };

            // ToDo remove after dev
            // Testing of maximum token length
            //var length = 0;
            //for (int i = 0; i < 100; i++)
            //{
            //    user = new User()
            //    {
            //        Id = Guid.NewGuid(),
            //        RoleId = _defaultRole,
            //        Email = request.Email,
            //        Username = request.UserName,
            //        PasswordHash = PasswordManager.GetHash(request.Password),
            //        Reputation = 0,
            //    };
            //    var token = JwtManager.GetRefreshToken(user);
            //    if (length < token.Length)
            //    {
            //        length = token.Length;
            //    }
            //}

            var result = _dataBaseService.GetProvider().AddUser(user);

            // ToDo Add more status codes
            if (!result)
            {
                return HttpStatusCode.Conflict;
            }

            return HttpStatusCode.Created;
        }
    }
}
