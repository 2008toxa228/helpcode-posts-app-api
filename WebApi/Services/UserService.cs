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
            _defaultRole = Guid.Parse("8e2edef6-8060-4cf4-a66b-6ab10f550dd1");
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

                // ToDo add refresh token to db.
                //_dataBaseService.GetProvider().AddRefreshToken();

                return response;
            }

            return null;
        }

        public async Task<HttpStatusCode> Register(RegisterRequest request)
        {
            var user = new User()
            {
                RoleId = _defaultRole,
                Email = request.Email,
                Username = request.UserName,
                PasswordHash = PasswordManager.GetHash(request.Password),
            };

            var result = _dataBaseService.GetProvider().AddUser(user);

            // ToDo Add more status codes
            if (result == 1)
            {
                return HttpStatusCode.Created;
            }

            return HttpStatusCode.Conflict;
        }
    }
}
