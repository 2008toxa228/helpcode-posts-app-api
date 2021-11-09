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
            if (request == null)
            {
                return null;
            }

            var user = _dataBaseService.GetProvider().GetUserByEmail(request.Email);

            if (user != null && PasswordManager.Verify(request.Password, user.PasswordHash))
            {
                var refreshToken = JwtManager.GetRefreshTokenEntity(user);

                return UpdateRefreshToken(refreshToken, user);
            }

            return null;
        }

        public AuthenticateResponse RefreshAccessToken(RefreshAccessTokenRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var token = _dataBaseService.GetProvider().GetRefreshToken(request.UserId);

            if (token?.Token == request.RefreshToken && JwtManager.ValidateToken(request.RefreshToken))
            {
                var user = _dataBaseService.GetProvider().GetUserById(request.UserId);

                var refreshToken = JwtManager.GetRefreshTokenEntity(user);

                return UpdateRefreshToken(refreshToken, user);
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

            var result = _dataBaseService.GetProvider().AddUser(user);

            // ToDo Add more status codes
            if (!result)
            {
                return HttpStatusCode.Conflict;
            }

            return HttpStatusCode.Created;
        }

        private AuthenticateResponse UpdateRefreshToken(RefreshToken refreshToken, User user)
        {
            if (!JwtManager.ValidateToken(refreshToken?.Token))
            {
                return null;
            }

            if (_dataBaseService.GetProvider().UpdateRefreshToken(refreshToken))
            {
                var response = new AuthenticateResponse(user, JwtManager.GetAccessToken(user), refreshToken.Token);

                return response;
            }

            // ToDo add logging of updating refreshToken error
            return null;
        }
    }
}
